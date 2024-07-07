using System.IO.Pipes;

// Create pipe client
if (args.Length < 1)
{
    Console.WriteLine("Invalid usage, a pipe handle is required");

    Environment.Exit(1);
}

using var pipeClient = new AnonymousPipeClientStream(PipeDirection.Out, args[0]);

// Build api app
var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.UseStaticFiles();

// Redirect index to settings form
app.MapGet("/", () => Results.Redirect("/index.html"));

// Handle settings change
app.MapPost("/settings", async context =>
{
    // Write settings to pipe, the randomizer will deserialize it itself
    // this avoids to share the settings model between this api and the randomizer
    if (context.Request.HasJsonContentType())
    {
        // ReSharper disable once AccessToDisposedClosure
        await context.Request.Body.CopyToAsync(pipeClient);
    }

    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
});

app.Run();

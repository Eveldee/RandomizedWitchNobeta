using System.Net;
using RandomizedWitchNobeta.WebSettings;

// ReSharper disable AccessToDisposedClosure

Console.WriteLine(string.Join(", ", args));

// Get settings path
var settingsPath = args is [var path, ..]
    ? path
    : "SeedSettings.json";

// Build api app
var builder = WebApplication.CreateSlimBuilder(args);

// Set port
builder.WebHost.ConfigureKestrel((_, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Loopback, 0);
});

builder.Services.AddHostedService<BrowserOpenService>();

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
        await using var outputStream = File.Create(settingsPath);

        await context.Request.Body.CopyToAsync(outputStream);
    }

    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
});

app.Run();

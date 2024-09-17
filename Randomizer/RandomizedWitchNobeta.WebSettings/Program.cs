using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using RandomizedWitchNobeta.Shared;
using RandomizedWitchNobeta.WebSettings;
using RandomizedWitchNobeta.WebSettings.Utils;

// ReSharper disable AccessToDisposedClosure

Console.WriteLine(string.Join(", ", args));

// Get seed settings path
var seedSettingsPath = args is [var path, ..]
    ? path
    : "SeedSettings.json";

var bonusSettingsPath = Path.Combine(Path.GetDirectoryName(seedSettingsPath)!, "BonusSettings.json");

// Ensure bonus settings file exists
if (!File.Exists(bonusSettingsPath))
{
    await File.WriteAllTextAsync(bonusSettingsPath, SerializeUtils.SerializeIndented(new BonusSettings()));
}

// Build api app
var builder = WebApplication.CreateSlimBuilder(args);

// Set port
builder.WebHost.ConfigureKestrel((_, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Loopback, 0);
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, JsonSourceGenerationContext.Default);
});

builder.Services.AddHostedService<BrowserOpenService>();

var app = builder.Build();

app.UseStaticFiles();

// Redirect index to settings form
app.MapGet("/", () => Results.Redirect("/index.html"));

// Handle settings change
app.MapPost("/settings", async (HttpRequest request) =>
{
    var json = await new StreamReader(request.Body).ReadToEndAsync();

    var bonusInitialized = JsonNode.Parse(json)?["BonusInitialized"]?.GetValue<bool>();

    if (SerializeUtils.DeserializeBonusSettings(json) is { } bonusSettings && bonusInitialized is true)
    {
        File.WriteAllText(bonusSettingsPath, SerializeUtils.SerializeIndented(bonusSettings));
    }

    if (SerializeUtils.DeserializeSeedSettings(json) is { } seedSettings)
    {
        File.WriteAllText(seedSettingsPath, SerializeUtils.SerializeIndented(seedSettings));

        return Results.Ok();
    }

    return Results.UnprocessableEntity();
});

app.MapGet("/bonus", async () =>
{
    if (File.Exists(bonusSettingsPath))
    {
        var content = await File.ReadAllTextAsync(bonusSettingsPath);

        return Results.Ok(SerializeUtils.DeserializeBonusSettings(content));
    }

    return Results.NotFound();
});

app.Run();

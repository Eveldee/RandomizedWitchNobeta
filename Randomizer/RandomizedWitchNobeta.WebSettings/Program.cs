using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Mvc;
using RandomizedWitchNobeta.Shared;
using RandomizedWitchNobeta.WebSettings;
using RandomizedWitchNobeta.WebSettings.Utils;
using TextCopy;

// ReSharper disable AccessToDisposedClosure

Console.WriteLine(string.Join(", ", args));

var messagePackOptions = MessagePackSerializerOptions.Standard.WithResolver(
    CompositeResolver.Create(
        GeneratedResolver.Instance,
        StandardResolver.Instance
    )
);

// Get seed settings path
var seedSettingsPath = args is [var path, ..]
    ? path
    : "SeedSettings.json";

int? parentProcessId = null;
if (args.Length >= 2 && int.TryParse(args[1], out var id))
{
    parentProcessId = id;
}

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
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddHostedService<BrowserOpenService>();
builder.Services.AddHostedService<AutoCloseService>(provider =>
    new AutoCloseService(provider.GetRequiredService<IHostApplicationLifetime>(), parentProcessId
));

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

app.MapPost("/clipboard-export", async (HttpRequest request) =>
{
    var json = await new StreamReader(request.Body).ReadToEndAsync();

    if (SerializeUtils.DeserializeSeedSettings(json) is { } seedSettings)
    {
        SettingsExporter.ExportSettings(seedSettings, messagePackOptions);

        return Results.Ok();
    }

    return Results.UnprocessableEntity();
});

app.MapGet("/clipboard-import", () =>
{
    if (SettingsExporter.TryImportSettings(messagePackOptions, out var seedSettings))
    {
        return Results.Ok(seedSettings);
    }

    return Results.NotFound();
});

app.Run();

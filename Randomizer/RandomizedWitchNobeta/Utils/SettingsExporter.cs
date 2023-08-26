using System;
using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using RandomizedWitchNobeta.Generation;
using TextCopy;

namespace RandomizedWitchNobeta.Utils;

public static class SettingsExporter
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        IncludeFields = true
    };

    public static void ExportSettings(SeedSettings seedSettings)
    {
        var json = JsonSerializer.Serialize(seedSettings, _serializerOptions);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        ClipboardService.SetText(base64);
    }

    public static bool TryImportSettings(out SeedSettings seedSettings)
    {
        var base64 = ClipboardService.GetText();

        try
        {
            var json = Convert.FromBase64String(base64!);

            seedSettings = JsonSerializer.Deserialize<SeedSettings>(json, _serializerOptions);

            return true;
        }
        catch (Exception)
        {
            seedSettings = null;
            return false;
        }
    }
}
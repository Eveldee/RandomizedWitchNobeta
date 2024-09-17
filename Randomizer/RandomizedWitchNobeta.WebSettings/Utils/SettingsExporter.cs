using System.Diagnostics.CodeAnalysis;
using MessagePack;
using RandomizedWitchNobeta.Shared;
using TextCopy;

namespace RandomizedWitchNobeta.WebSettings.Utils;

public static class SettingsExporter
{
    public static void ExportSettings(SeedSettings seedSettings)
    {
        var data = MessagePackSerializer.Serialize(seedSettings);
        var base64 = Convert.ToBase64String(data);
        ClipboardService.SetText(base64);
    }

    public static bool TryImportSettings([NotNullWhen(true)] out SeedSettings? seedSettings)
    {
        var base64 = ClipboardService.GetText();

        try
        {
            var data = Convert.FromBase64String(base64!);

            seedSettings = MessagePackSerializer.Deserialize<SeedSettings>(data);

            return true;
        }
        catch (Exception)
        {
            seedSettings = null;
            return false;
        }
    }
}
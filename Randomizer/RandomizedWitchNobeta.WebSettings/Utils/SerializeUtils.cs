using System.Text.Json;
using System.Text.Json.Serialization;
using RandomizedWitchNobeta.Shared;

namespace RandomizedWitchNobeta.WebSettings.Utils;

[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, UseStringEnumConverter = true, PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified)]
[JsonSerializable(typeof(SeedSettings))]
[JsonSerializable(typeof(BonusSettings))]
internal partial class JsonSourceGenerationContext : JsonSerializerContext;

internal static class SerializeUtils
{
    public static string SerializeIndented(SeedSettings value)
    {
        return JsonSerializer.Serialize(value, JsonSourceGenerationContext.Default.SeedSettings);
    }
    public static string SerializeIndented(BonusSettings value)
    {
        return JsonSerializer.Serialize(value, JsonSourceGenerationContext.Default.BonusSettings);
    }

    public static SeedSettings? DeserializeSeedSettings(string json)
    {
        return JsonSerializer.Deserialize(json, JsonSourceGenerationContext.Default.SeedSettings);
    }
    public static BonusSettings? DeserializeBonusSettings(string json)
    {
        return JsonSerializer.Deserialize(json, JsonSourceGenerationContext.Default.BonusSettings);
    }
}
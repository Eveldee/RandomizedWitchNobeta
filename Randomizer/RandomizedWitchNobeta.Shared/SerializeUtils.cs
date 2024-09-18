using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizedWitchNobeta.Shared;

internal static class SerializeUtils
{
    private static readonly JsonSerializerOptions _options = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        },
        WriteIndented = true
    };

    public static string SerializeIndented<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, _options);
    }
}
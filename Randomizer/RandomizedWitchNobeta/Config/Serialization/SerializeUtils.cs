using System.Text.Json;
using System.Text.Json.Serialization;
using TupleAsJsonArray;

namespace RandomizedWitchNobeta.Config.Serialization;

public static class SerializeUtils
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new UnityVector3JsonConverter(),
            new UnityQuaternionJsonConverter(),
            new NumericsVector4JsonConverter(),
            new TupleConverterFactory(),
            new DictionaryPairListConverter()
        }
    };

    public static string SerializeIndented<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    public static TValue Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, _options);
    }
}
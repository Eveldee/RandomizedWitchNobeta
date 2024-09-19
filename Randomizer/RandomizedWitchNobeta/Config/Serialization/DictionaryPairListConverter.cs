using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizedWitchNobeta.Config.Serialization;

public class DictionaryPairListConverter : JsonConverterFactory
{
    private readonly HashSet<Type> _defaultSupportedKeyTypes =
    [
        typeof(bool),
        typeof(byte),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(decimal),
        typeof(double),
        typeof(Enum),
        typeof(Guid),
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(sbyte),
        typeof(float),
        typeof(string),
        typeof(ushort),
        typeof(uint),
        typeof(ulong)
    ];

    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        if (typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>))
        {
            return false;
        }

        // Only allow non-supported key types
        var keyType = typeToConvert.GetGenericArguments()[0];

        if (_defaultSupportedKeyTypes.Contains(keyType))
        {
            return false;
        }

        // For all other types use this converter
        return true;
    }

    public override JsonConverter CreateConverter(
        Type type,
        JsonSerializerOptions options)
    {
        var typeArguments = type.GetGenericArguments();
        Type keyType = typeArguments[0];
        Type valueType = typeArguments[1];

        JsonConverter converter = (JsonConverter) Activator.CreateInstance(
            typeof(DictionaryPairListConverterInner<,>).MakeGenericType(keyType, valueType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: [options],
            culture: null
        )!;

        return converter;
    }

    private class DictionaryPairListConverterInner<TKey, TValue> :
        JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
    {
        private readonly JsonConverter<List<KeyValuePair<TKey, TValue>>> _pairListConverter;
        private readonly Type _pairListType;

        public DictionaryPairListConverterInner(JsonSerializerOptions options)
        {
            _pairListType = typeof(List<KeyValuePair<TKey, TValue>>);

            // For performance, use the existing converter.
            _pairListConverter = (JsonConverter<List<KeyValuePair<TKey, TValue>>>) options
                .GetConverter(_pairListType);
        }

        public override Dictionary<TKey, TValue> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<TKey, TValue>();

            var pairList = _pairListConverter.Read(ref reader, _pairListType, options)!;

            foreach (var (key, value) in pairList)
            {
                dictionary[key] = value;
            }

            return dictionary;
        }

        public override void Write(
            Utf8JsonWriter writer,
            Dictionary<TKey, TValue> dictionary,
            JsonSerializerOptions options)
        {
            var pairList = dictionary.ToList();

            _pairListConverter.Write(writer, pairList, options);
        }
    }
}
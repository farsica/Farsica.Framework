namespace Farsica.Framework.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Data.Enumeration;

    public class DictionaryEnumerationConverter<TEnum, TKey> : JsonConverter<Dictionary<Enumeration<TEnum, TKey>, object?>>
        where TKey : IEquatable<TKey>, IComparable<TKey>
        where TEnum : Enumeration<TEnum, TKey>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<Enumeration<TEnum, TKey>, object?>);
        }

        public override Dictionary<Enumeration<TEnum, TKey>, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<Enumeration<TEnum, TKey>, object?>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();

                if (!propertyName.TryGetFromNameOrValue<TEnum, TKey>(out TEnum? enumeration) || enumeration is null)
                {
                    throw new JsonException($"Unable to convert \"{propertyName}\" to Enumeration \"{typeof(Enumeration<,>)}\".");
                }

                // Get the value.
                reader.Read();
                object? value = (options.GetConverter(typeof(object)) as JsonConverter<object?>)?.Read(ref reader, typeof(object), options);

                // Add to dictionary.
                dictionary.Add(enumeration, value);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Enumeration<TEnum, TKey>, object?> dictionary, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach ((Enumeration<TEnum, TKey> key, object? value) in dictionary)
            {
                var propertyName = key.Name!;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName);

                (options.GetConverter(typeof(object)) as JsonConverter<object?>)?.Write(writer, value, options);
            }

            writer.WriteEndObject();
        }
    }
}

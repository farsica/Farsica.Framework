namespace Farsica.Framework.Converter
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Data.Enumeration;

    public class EnumerationJsonConverter<TEnum, TKey> : JsonConverter<TEnum>
        where TEnum : Enumeration<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        private const string NameProperty = "Name";

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(Enumeration<TKey>));
        }

        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                case JsonTokenType.String:
                    return GetEnumerationFromJson(reader.GetString());
                case JsonTokenType.Null:
                    return null;
                default:
                    throw new JsonException(
                        $"Unexpected token {reader.TokenType} when parsing the enumeration.");
            }
        }

        /// <summary>
        /// Writes a specified <see cref="Enumeration"/> value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to the JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNull(NameProperty);
            }
            else
            {
                var name = value.GetType().GetProperty(NameProperty, BindingFlags.Public | BindingFlags.Instance);
                if (name is null)
                {
                    throw new JsonException($"Error while writing JSON for {value}");
                }

                writer.WriteStringValue(name.GetValue(value)?.ToString());
            }
        }

        private static TEnum? GetEnumerationFromJson(string nameOrValue)
        {
            nameOrValue.TryGetFromNameOrValue<TEnum, TKey>(out TEnum? result);

            return result;
        }
    }
}

namespace Farsica.Framework.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Data.Enumeration;

    public class FlagsEnumerationConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : FlagsEnumeration<TEnum>, new()
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(FlagsEnumeration<TEnum>));
        }

        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (TEnum?)(object?)null;
            }

            if (reader.TokenType is not JsonTokenType.StartArray)
            {
                reader.Skip();
                return reader.GetString()?.FromName<TEnum>() ?? reader.GetString()?.FromUniqueId<TEnum>();
            }

            var list = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                var val = reader.GetString();
                if (string.IsNullOrEmpty(val))
                {
                    continue;
                }

                list.Add(val);
            }

            return list.ListToFlagsEnum<TEnum>();
        }

        /// <summary>
        /// Writes a specified <see cref="FlagsEnumeration"/> value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to the JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonSerializer.Serialize(FlagsEnumerationExtensions.GetNames(value)));
        }
    }
}

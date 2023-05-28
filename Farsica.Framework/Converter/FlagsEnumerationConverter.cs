namespace Farsica.Framework.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Data.Enumeration;

    public class FlagsEnumerationConverter<TEnum> : JsonConverter<Flag<TEnum>>
        where TEnum : FlagsEnumeration<TEnum>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(FlagsEnumeration<TEnum>));
        }

        public override Flag<TEnum>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString()?.FromName<TEnum>();
        }

        /// <summary>
        /// Writes a specified <see cref="FlagsEnumeration"/> value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to the JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, Flag<TEnum> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniqueId());
        }
    }
}

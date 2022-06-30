namespace Farsica.Framework.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Core;

    public class EnumDisplayNameConverter<T> : JsonConverter<T>
    {
        private readonly JsonConverter<T> converter;
        private readonly Type underlyingType;

        public EnumDisplayNameConverter()
            : this(null)
        {
        }

        public EnumDisplayNameConverter(JsonSerializerOptions options)
        {
            if (options != null)
            {
                converter = options.GetConverter(typeof(T)) as JsonConverter<T>;
            }

            underlyingType = Nullable.GetUnderlyingType(typeof(T));
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (converter != null)
            {
                return converter.Read(ref reader, underlyingType, options);
            }

            string value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            foreach (var item in Enum.GetNames(typeToConvert))
            {
                if (EnumHelper.LocalizeEnum(item) == value)
                {
                    return (T)Enum.Parse(underlyingType, item, ignoreCase: false);
                }
            }

            throw new JsonException($"Unable to convert \"{value}\" to Enum \"{underlyingType}\".");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(EnumHelper.LocalizeEnum(value));
        }
    }
}

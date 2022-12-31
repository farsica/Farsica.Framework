namespace Farsica.Framework.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class EnumStringConverter<T> : JsonConverter<T>
    {
        private readonly JsonConverter<T>? converter;
        private readonly Type underlyingType;

        public EnumStringConverter()
            : this(null)
        {
        }

        public EnumStringConverter(JsonSerializerOptions? options)
        {
            if (options is not null)
            {
                converter = options.GetConverter(typeof(T)) as JsonConverter<T>;
            }

            var type = Nullable.GetUnderlyingType(typeof(T));
            if (type is null)
            {
                throw new ArgumentException(nameof(T));
            }

            underlyingType = type;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (converter is not null)
            {
                return converter.Read(ref reader, underlyingType, options);
            }

            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            // for performance, parse with ignoreCase:false first.
            if (!System.Enum.TryParse(underlyingType, value, ignoreCase: false, out object? result)
                && !System.Enum.TryParse(underlyingType, value, ignoreCase: true, out result))
            {
                throw new JsonException($"Unable to convert \"{value}\" to Enum \"{underlyingType}\".");
            }

            return (T?)result;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}

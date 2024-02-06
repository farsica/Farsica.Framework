namespace Farsica.Framework.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Resources;
    using NUlid;

    public class UlidJsonConverter : JsonConverter<Ulid>
    {
        public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new ArgumentException("TokenType");
            }

            if (Ulid.TryParse(reader.GetString() ?? string.Empty, out Ulid ulid))
            {
                return ulid;
            }

            throw new ArgumentException(string.Format(GlobalResource.Validation_ValueIsInvalidAccessor, reader.GetString()));
        }

        public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Ulid);
        }
    }
}

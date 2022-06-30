namespace Farsica.Framework.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && decimal.TryParse(reader.GetString(), out decimal number))
            {
                return number;
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetDecimal(out number))
            {
                return number;
            }

            throw new ArgumentException("TokenType");
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return Type.GetTypeCode(typeToConvert) == TypeCode.Decimal;
        }
    }
}

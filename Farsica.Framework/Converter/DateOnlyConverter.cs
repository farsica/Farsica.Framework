namespace Farsica.Framework.Converter
{
    using System;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy/MM/dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateOnly.ParseExact(reader.GetString()!, DateFormat, CultureInfo.InvariantCulture);

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}

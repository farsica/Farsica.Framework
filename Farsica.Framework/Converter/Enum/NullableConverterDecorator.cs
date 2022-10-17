namespace Farsica.Framework.Converter.Enum
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public sealed class NullableConverterDecorator<T> : JsonConverter<T?>
        where T : struct
    {
        // Read() and Write() are never called with null unless HandleNull is overwridden -- which it is not.
        private readonly JsonConverter<T> innerConverter;

        public NullableConverterDecorator(JsonConverter<T> innerConverter) => this.innerConverter = innerConverter ?? throw new ArgumentNullException(nameof(innerConverter));

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => innerConverter.Read(ref reader, Nullable.GetUnderlyingType(typeToConvert)!, options);

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) => innerConverter.Write(writer, value!.Value, options);

        public override bool CanConvert(Type type) => base.CanConvert(type) && innerConverter.CanConvert(Nullable.GetUnderlyingType(type)!);
    }
}

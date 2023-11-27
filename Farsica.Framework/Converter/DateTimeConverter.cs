namespace Farsica.Framework.Converter
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.TimeZone;
    using Microsoft.Extensions.DependencyInjection;
    using static Farsica.Framework.Core.Constants;

    public class DateTimeConverter<T> : JsonConverter<T>
    {
        private readonly string? format;
        private readonly FormatProvider formatProvider;

        public DateTimeConverter(string? format, FormatProvider? formatProvider)
        {
            this.format = format;
            this.formatProvider = formatProvider ?? FormatProvider.CurrentCulture;
        }

        public override bool HandleNull => true;

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IFormatProvider provider = formatProvider == FormatProvider.CurrentCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;

            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            {
                return (T?)(object?)reader.GetDateTime();
            }
            else if (typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?))
            {
                return (T?)(object?)new DateTimeOffset(reader.GetDateTime(), GetTimeSpan(options));
            }
            else if (typeToConvert == typeof(DateOnly) || typeToConvert == typeof(DateOnly?))
            {
                var val = reader.GetString();
                return string.IsNullOrEmpty(val) ? (T?)(object?)null : (T?)(object?)DateOnly.ParseExact(val, format ?? "yyyy/MM/dd", provider);
            }
            else if (typeToConvert == typeof(TimeOnly) || typeToConvert == typeof(TimeOnly?))
            {
                var val = reader.GetString();
                return string.IsNullOrEmpty(val) ? (T?)(object?)null : (T?)(object?)TimeOnly.ParseExact(val, format ?? "HH:mm:ss", provider);
            }

            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            IFormatProvider provider = formatProvider == FormatProvider.CurrentCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;

            if (value is DateTimeOffset offset)
            {
                writer.WriteStringValue(offset.UtcDateTime.Add(GetTimeSpan(options)).ToString(format, provider));
            }
            else
            {
                writer.WriteStringValue((value as ISpanFormattable)?.ToString(format, provider));
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime) ||
                typeToConvert == typeof(DateTime?) ||

                typeToConvert == typeof(DateTimeOffset) ||
                typeToConvert == typeof(DateTimeOffset?) ||

                typeToConvert == typeof(DateOnly) ||
                typeToConvert == typeof(DateOnly?) ||

                typeToConvert == typeof(TimeOnly) ||
                typeToConvert == typeof(TimeOnly?);
        }

        private static TimeSpan GetTimeSpan(JsonSerializerOptions options)
        {
            var converter = options.Converters.OfType<ServiceProviderDummyConverter>().FirstOrDefault();
            var timeZoneId = converter?.HttpContextAccessor.HttpContext?.User.FindFirstValue(TimeZoneIdClaim) ?? IranTimeZoneId;
            return converter?.ServiceProvider?.GetRequiredService<ITimeZoneProvider>().GetTimeZones()?.FirstOrDefault(t => t.Id == timeZoneId)?.BaseUtcOffset ?? IranBaseUtcOffset;
        }
    }
}

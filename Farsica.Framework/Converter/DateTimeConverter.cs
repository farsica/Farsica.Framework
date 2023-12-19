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
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)dt;
                }

                return (T?)(object?)null;
            }
            else if (typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)new DateTimeOffset(dt, GetTimeSpan(options));
                }

                return (T?)(object?)null;
            }
            else if (typeToConvert == typeof(DateOnly) || typeToConvert == typeof(DateOnly?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)DateOnly.FromDateTime(dt);
                }

                return (T?)(object?)null;
            }
            else if (typeToConvert == typeof(TimeOnly) || typeToConvert == typeof(TimeOnly?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)TimeOnly.FromDateTime(dt);
                }

                var val = reader.GetString();
                var timeFormat = string.IsNullOrEmpty(format) ? "HH:mm:ss" : format;
                if (!string.IsNullOrEmpty(val) && TimeOnly.TryParseExact(val, timeFormat, provider, DateTimeStyles.None, out TimeOnly result))
                {
                    return (T?)(object?)result;
                }

                return (T?)(object?)null;
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

namespace Farsica.Framework.Converter.DateTimeOffset
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Core;
    using Farsica.Framework.TimeZone;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class DateTimeOffsetConverterBase : JsonConverter<DateTimeOffset>
    {
        public abstract string Format { get; }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new DateTimeOffset(reader.GetDateTime(), GetTimeSpan(options));
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.UtcDateTime.Add(GetTimeSpan(options)).ToString(Format));
        }

        private static TimeSpan GetTimeSpan(JsonSerializerOptions options)
        {
            var converter = options.Converters.OfType<ServiceProviderDummyConverter>().FirstOrDefault();
            var timeZoneId = converter?.HttpContextAccessor.HttpContext?.User.FindFirstValue(Constants.TimeZoneIdClaim) ?? Constants.IranTimeZoneId;
            return converter?.ServiceProvider?.GetRequiredService<ITimeZoneProvider>().GetTimeZones()?.FirstOrDefault(t => t.Id == timeZoneId)?.BaseUtcOffset ?? Constants.IranBaseUtcOffset;
        }
    }
}

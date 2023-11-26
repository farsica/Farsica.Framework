namespace Farsica.Framework.Converter
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using Farsica.Framework.DataAnnotation;

    internal class CustomtJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var jsonTypeInfo = base.GetTypeInfo(type, options);

            var lst = jsonTypeInfo.Properties.Where(t => IsValidType(t.PropertyType));
            if (lst?.Any() == true)
            {
                foreach (var prop in lst)
                {
                    var displayFormat = (prop.AttributeProvider as MemberInfo)?.GetCustomAttribute<DisplayFormatAttribute>();
                    if (displayFormat is null)
                    {
                        continue;
                    }

                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTime?>(displayFormat.DataFormatString, displayFormat.FormatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateTimeOffset) || prop.PropertyType == typeof(DateTimeOffset?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTimeOffset?>(displayFormat.DataFormatString, displayFormat.FormatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateOnly) || prop.PropertyType == typeof(DateOnly?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateOnly?>(displayFormat.DataFormatString, displayFormat.FormatProvider);
                    }
                    else if (prop.PropertyType == typeof(TimeOnly) || prop.PropertyType == typeof(TimeOnly?))
                    {
                        prop.CustomConverter = new DateTimeConverter<TimeOnly?>(displayFormat.DataFormatString, displayFormat.FormatProvider);
                    }
                }
            }

            return jsonTypeInfo;
        }

        private static bool IsValidType(Type type)
        {
            var lst = new[] { typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?), typeof(DateOnly), typeof(DateOnly?), typeof(TimeOnly), typeof(TimeOnly?) };
            return lst.Contains(type);
        }
    }
}

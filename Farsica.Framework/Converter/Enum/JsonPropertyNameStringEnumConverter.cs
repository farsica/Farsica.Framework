namespace Farsica.Framework.Converter.Enum
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Core.Extensions;

    public class JsonPropertyNameStringEnumConverter : GeneralJsonStringEnumConverter
    {
        public JsonPropertyNameStringEnumConverter()
            : base()
        {
        }

        public JsonPropertyNameStringEnumConverter(JsonNamingPolicy? namingPolicy = default, bool allowIntegerValues = true)
            : base(namingPolicy, allowIntegerValues)
        {
        }

        protected override bool TryOverrideName(Type enumType, string name, out ReadOnlyMemory<char> overrideName)
        {
            if (JsonEnumExtensions.TryGetEnumAttribute<JsonPropertyNameAttribute>(enumType, name, out var attr) && attr.Name != null)
            {
                overrideName = attr.Name.AsMemory();
                return true;
            }

            return base.TryOverrideName(enumType, name, out overrideName);
        }
    }
}

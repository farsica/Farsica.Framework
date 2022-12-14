namespace Farsica.Framework.Converter.Enum
{
    using System;
    using System.Text.Json;
    using Farsica.Framework.Core.Extensions;

    public class JsonEnumMemberStringEnumConverter : GeneralJsonStringEnumConverter
    {
        public JsonEnumMemberStringEnumConverter()
            : base()
        {
        }

        public JsonEnumMemberStringEnumConverter(JsonNamingPolicy? namingPolicy = default, bool allowIntegerValues = true)
            : base(namingPolicy, allowIntegerValues)
        {
        }

        protected override bool TryOverrideName(Type enumType, string? name, out ReadOnlyMemory<char> overrideName)
        {
            if (JsonEnumExtensions.TryGetEnumAttribute<System.Runtime.Serialization.EnumMemberAttribute>(enumType, name, out var attr) && attr.Value != null)
            {
                overrideName = attr.Value.AsMemory();
                return true;
            }

            return base.TryOverrideName(enumType, name, out overrideName);
        }
    }
}

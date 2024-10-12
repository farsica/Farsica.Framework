namespace Farsica.Framework.Core.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public static class TypeExtensions
    {
        public static bool IsSimpleType([NotNullWhen(returnValue: false)] this Type? type)
        {
            return
                type is null ||
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[]
                {
                typeof(string),
                typeof(decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid),
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}

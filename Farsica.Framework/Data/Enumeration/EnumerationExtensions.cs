namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Farsica.Framework.Core.Extensions;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class EnumerationExtensions
    {
        public static IEnumerable<T?>? GetAll<T>()
            where T : Enumeration
        {
            var fields = typeof(T).GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(t => t.GetValue(null).As<T>());
        }

        public static IEnumerable<object?>? GetAll(Type type)
        {
            var fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(t => t.GetValue(null));
        }

        public static int AbsoluteDifference<T>(this T firstValue, Enumeration secondValue)
            where T : Enumeration
        {
            return Math.Abs(firstValue.Value - secondValue.Value);
        }

        public static bool TryGetFromValueOrName<T>(this string? nameOrValue, out T? enumeration)
            where T : Enumeration
        {
            return TryParse(t => t.Name == nameOrValue, out enumeration) ||
                   (int.TryParse(nameOrValue, out var value) && TryParse(t => t.Value == value, out enumeration));
        }

        public static T? ToEnumeration<T>(this int value)
            where T : Enumeration
        {
            var item = GetAll<T>()?.FirstOrDefault(t => t?.Value == value);
            if (item is null)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return item;
        }

        public static T? ToEnumeration<T>(this string? name)
            where T : Enumeration
        {
            var item = GetAll<T>()?.FirstOrDefault(t => t?.Name == name);
            if (item is null)
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            return item;
        }

        public static void OwnEnumeration<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum?>> property)
            where TEntity : class
            where TEnum : Enumeration
        {
            _ = builder.Property(property).HasConversion(t => t.Value, t => t.ToEnumeration<TEnum>());
        }

        private static bool TryParse<T>(Func<T, bool> predicate, out T? enumeration)
            where T : Enumeration
        {
            enumeration = GetAll<T>()?.FirstOrDefault(predicate);
            return enumeration is not null;
        }
    }
}

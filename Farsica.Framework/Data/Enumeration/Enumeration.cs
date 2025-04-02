﻿namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Farsica.Framework.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public abstract class Enumeration<TEnum, TKey> : IComparable, IEquatable<Enumeration<TEnum, TKey>>, IComparable<Enumeration<TEnum, TKey>>, IRouteConstraint
        where TKey : IEquatable<TKey>, IComparable<TKey>
        where TEnum : Enumeration<TEnum, TKey>
    {
        [ExcludeFromCodeCoverage]
        protected Enumeration()
        {
        }

        protected Enumeration([NotNull] string name, TKey value)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; set; }

        public TKey Value { get; set; }

        public string? LocalizedDisplayName => Globals.GetLocalizedDisplayName(GetType().GetField(Name));

        public string? LocalizedShortName => Globals.GetLocalizedShortName(GetType().GetField(Name));

        public string? LocalizedDescription => Globals.GetLocalizedDescription(GetType().GetField(Name));

        public string? LocalizedPromt => Globals.GetLocalizedPromt(GetType().GetField(Name));

        public string? LocalizedGroupName => Globals.GetLocalizedGroupName(GetType().GetField(Name));

        public static bool operator ==(Enumeration<TEnum, TKey>? left, Enumeration<TEnum, TKey>? right) => Equals(left, right);

        public static bool operator !=(Enumeration<TEnum, TKey>? left, Enumeration<TEnum, TKey>? right) => !Equals(left, right);

        public static bool operator <(Enumeration<TEnum, TKey> left, Enumeration<TEnum, TKey> right) => left is null ? right is not null : left.CompareTo(right) < 0;

        public static bool operator <=(Enumeration<TEnum, TKey> left, Enumeration<TEnum, TKey> right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Enumeration<TEnum, TKey> left, Enumeration<TEnum, TKey> right) => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(Enumeration<TEnum, TKey> left, Enumeration<TEnum, TKey> right) => left is null ? right is null : left.CompareTo(right) >= 0;

        public override string? ToString() => Name;

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object? obj) => obj is Enumeration<TEnum, TKey> && Equals(obj as Enumeration<TEnum, TKey>);

        public bool Equals(Enumeration<TEnum, TKey>? other) => other is not null && (ReferenceEquals(this, other) || Value.Equals(other.Value));

        public int CompareTo(Enumeration<TEnum, TKey>? other) => Value.CompareTo(other!.Value);

        public int CompareTo(object? obj) => Value.CompareTo((obj as Enumeration<TEnum, TKey>)!.Value);

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            ArgumentNullException.ThrowIfNull(routeKey);
            ArgumentNullException.ThrowIfNull(values);

            if (values.TryGetValue(routeKey, out var value) && value != null)
            {
                if (value is TEnum)
                {
                    return true;
                }

                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                return valueString.TryGetFromNameOrValue<TEnum, TKey>(out _);
            }

            return false;
        }
    }
}

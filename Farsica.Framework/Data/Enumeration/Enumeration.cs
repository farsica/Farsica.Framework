namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Farsica.Framework.Core;

    public abstract class Enumeration<T> : IComparable, IEquatable<Enumeration<T>>, IComparable<Enumeration<T>>
        where T : IEquatable<T>, IComparable<T>
    {
        [ExcludeFromCodeCoverage]
        protected Enumeration()
        {
        }

        protected Enumeration(string name, T value)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; set; }

        public T Value { get; set; }

        public string? LocalizedDisplayName => Globals.GetLocalizedDisplayName(GetType().GetField(Name));

        public string? LocalizedShortName => Globals.GetLocalizedShortName(GetType().GetField(Name));

        public string? LocalizedDescription => Globals.GetLocalizedDescription(GetType().GetField(Name));

        public string? LocalizedPromt => Globals.GetLocalizedPromt(GetType().GetField(Name));

        public string? LocalizedGroupName => Globals.GetLocalizedGroupName(GetType().GetField(Name));

        public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
        {
            return !Equals(left, right);
        }

        public override string? ToString() => Name;

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<T> && Equals(obj as Enumeration<T>);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(Enumeration<T>? other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(Enumeration<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Value.Equals(other.Value);
        }

        public int CompareTo(object? other)
        {
            return Value.CompareTo((other as Enumeration<T>).Value);
        }
    }
}

namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public abstract class FlagsEnumeration<TEnum, TKey> : Enumeration<TKey>
        where TEnum : FlagsEnumeration<TEnum, TKey>?
        where TKey : IEquatable<TKey>, IComparable<TKey>?, IBitwiseOperators<TKey, TKey, TKey>?, IEqualityOperators<TKey, TKey, bool>?
    {
#pragma warning disable SA1401 // Fields should be private
        protected readonly IDictionary<string, TKey> Types = new SortedDictionary<string, TKey>();
#pragma warning restore SA1401 // Fields should be private

        protected FlagsEnumeration(string name, TKey value)
            : base(name, value)
        {
            Types[name] = value;
        }

        protected FlagsEnumeration()
        {
        }

        internal IEnumerable<string> Names => Types.Select(t => t.Key);

        public static TEnum operator |(FlagsEnumeration<TEnum, TKey> left, TEnum right)
        {
            return left.Or(right);
        }

        public virtual bool HasFlag(TEnum flag)
        {
            return flag is not null && (Value & flag.Value) == flag.Value;
        }

        public virtual bool HasFlagValue(TKey value)
        {
            return (Value & value) == value;
        }

        protected abstract TEnum Or(TEnum other);
    }
}

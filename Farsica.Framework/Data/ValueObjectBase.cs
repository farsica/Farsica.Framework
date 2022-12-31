namespace Farsica.Framework.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ValueObjectBase
    {
        public static bool operator ==(ValueObjectBase? left, ValueObjectBase? right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(ValueObjectBase? left, ValueObjectBase? right)
        {
            return NotEqualOperator(left, right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not ValueObjectBase other)
            {
                return false;
            }

            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current is not null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
             .Select(t => t is not null ? t.GetHashCode() : 0)
             .Aggregate((t1, t2) => t1 ^ t2);
        }

        protected static bool EqualOperator(ValueObjectBase? left, ValueObjectBase? right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObjectBase? left, ValueObjectBase? right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetAtomicValues();
    }
}

namespace Farsica.Framework.DataAccess.Specification.Enumerable
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndEnumerableSpecification<T>(left, right);
        }

        public static ISpecification<T> AndNot<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndNotEnumerableSpecification<T>(left, right);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new OrEnumerableSpecification<T>(left, right);
        }

        public static ISpecification<T> OrNot<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new OrNotEnumerableSpecification<T>(left, right);
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> specification)
        {
            return new NotEnumerableSpecification<T>(specification);
        }
    }
}

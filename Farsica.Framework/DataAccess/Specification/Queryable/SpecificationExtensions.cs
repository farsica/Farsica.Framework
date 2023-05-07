namespace Farsica.Framework.DataAccess.Specification.Queryable
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndQueryableSpecification<T>(left, right);
        }

        public static ISpecification<T> AndNot<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndNotQueryableSpecification<T>(left, right);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new OrQueryableSpecification<T>(left, right);
        }

        public static ISpecification<T> OrNot<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new OrNotQueryableSpecification<T>(left, right);
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> specification)
        {
            return new NotQueryableSpecification<T>(specification);
        }
    }
}

namespace Farsica.Framework.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : SpecificationBase<T>
    {
        private readonly ISpecification<T> left = left;
        private readonly ISpecification<T> right = right;

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().And(right.Expression());
        }
    }
}

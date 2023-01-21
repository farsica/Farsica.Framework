namespace Farsica.Framework.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class AndNotSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> left;
        private readonly ISpecification<T> right;

        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().AndNot(right.Expression());
        }
    }
}

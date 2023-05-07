namespace Farsica.Framework.DataAccess.Specification.Enumerable
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class NotEnumerableSpecification<T> : EnumerableSpecification<T>
    {
        private readonly ISpecification<T> left;

        public NotEnumerableSpecification(ISpecification<T> left) => this.left = left;

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().Not();
        }
    }
}

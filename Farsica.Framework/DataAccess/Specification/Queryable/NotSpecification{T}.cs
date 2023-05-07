namespace Farsica.Framework.DataAccess.Specification.Queryable
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class NotQueryableSpecification<T> : QueryableSpecification<T>
    {
        private readonly ISpecification<T> left;

        public NotQueryableSpecification(ISpecification<T> left) => this.left = left;

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().Not();
        }
    }
}

namespace Farsica.Framework.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class NotSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> left;

        public NotSpecification(ISpecification<T> left) => this.left = left;

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().Not();
        }
    }
}

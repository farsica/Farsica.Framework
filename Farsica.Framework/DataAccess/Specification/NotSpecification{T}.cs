namespace Farsica.Framework.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.Core.Extensions.Linq;

    public class NotSpecification<T>(ISpecification<T> left) : SpecificationBase<T>
    {
        private readonly ISpecification<T> left = left;

        public override Expression<Func<T, bool>> Expression()
        {
            return left.Expression().Not();
        }
    }
}

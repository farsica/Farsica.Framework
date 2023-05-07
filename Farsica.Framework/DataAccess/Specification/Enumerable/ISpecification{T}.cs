namespace Farsica.Framework.DataAccess.Specification.Enumerable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Farsica.Framework.Data;

    public interface ISpecification<T>
    {
        PageFilter? PageFilter { get; }

        Func<IEnumerable<T>, IOrderedEnumerable<T>>? Order { get; }

        Expression<Func<T, bool>> Expression();

        bool IsSatisfiedBy(T candidate);
    }
}

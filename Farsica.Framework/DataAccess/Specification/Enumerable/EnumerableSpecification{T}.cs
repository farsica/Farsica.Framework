namespace Farsica.Framework.DataAccess.Specification.Enumerable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Farsica.Framework.Data;

    public abstract class EnumerableSpecification<T> : ISpecification<T>
    {
        public virtual Func<IEnumerable<T>, IOrderedEnumerable<T>>? Order => null;

        public virtual PageFilter? PageFilter => null;

        public abstract Expression<Func<T, bool>> Expression();

        public bool IsSatisfiedBy(T candidate) => Expression().Compile()(candidate);
    }
}

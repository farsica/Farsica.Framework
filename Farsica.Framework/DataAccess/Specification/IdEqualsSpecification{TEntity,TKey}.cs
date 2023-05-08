namespace Farsica.Framework.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.DataAccess.Entities;

    public sealed class IdEqualsSpecification<TEntity, TKey> : SpecificationBase<TEntity>
        where TEntity : class, IEntity<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TKey id;

        public IdEqualsSpecification(TKey id)
        {
            this.id = id;
        }

        public override Expression<Func<TEntity, bool>> Expression() => t => t.Id.Equals(id);
    }
}

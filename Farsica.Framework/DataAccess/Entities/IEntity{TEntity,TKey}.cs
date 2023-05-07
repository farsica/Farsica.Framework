namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public interface IEntity<TEntity, TKey> : IIdentifiable<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
    }
}

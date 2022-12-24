namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore;

    public interface IEntity<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}

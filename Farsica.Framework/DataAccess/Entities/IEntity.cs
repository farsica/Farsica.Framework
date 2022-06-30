namespace Farsica.Framework.DataAccess.Entities
{
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore;

    public interface IEntity<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}
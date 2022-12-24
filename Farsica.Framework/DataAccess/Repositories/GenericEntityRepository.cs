namespace Farsica.Framework.DataAccess.Repositories
{
    using System;
    using Farsica.Framework.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class GenericEntityRepository<TEntity, TKey> : EntityRepositoryBase<DbContext, TEntity, TKey>
        where TEntity : class, IEntity<TEntity, TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public GenericEntityRepository(ILogger<DataAccess> logger)
            : base(logger, null)
        {
        }
    }
}

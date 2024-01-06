namespace Farsica.Framework.DataAccess.Repositories
{
    using System;
    using Farsica.Framework.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class GenericEntityRepository<TEntity, TKey>(ILogger<DataAccess> logger) : EntityRepositoryBase<DbContext, TEntity, TKey>(logger, null)
        where TEntity : class, IEntity<TEntity, TKey>, new()
        where TKey : IEquatable<TKey>
    {
    }
}

namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAccess.Bulk;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.Repositories;

    public interface IUnitOfWorkBase : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepository<TEntity, long> GetRepository<TEntity>()
            where TEntity : class, IEntity<TEntity, long>;

        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TEntity, TKey>
            where TKey : IEquatable<TKey>;

        TRepository GetCustomRepository<TRepository>();

        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<(string ParameterName, object Value)>? param = null);

        Task<DataSet> SqlQueryAsync(string sql, IList<(string ParameterName, object Value)>? param = null);

        Task<int> GetSequenceValueAsync(string sequence);

        int GetSequenceValue(string sequence);

        #region Bulk Operation

        Task BulkInsertAsync<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TEntity : class;

        void BulkInsert<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TEntity : class;

        Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TEntity : class;

        void BulkUpdate<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TEntity : class;

        Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TEntity : class;

        void BulkDelete<TEntity>(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TEntity : class;

        #endregion
    }
}

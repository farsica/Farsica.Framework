namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
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
    }
}

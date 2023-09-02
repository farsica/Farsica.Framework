namespace Farsica.Framework.DataAccess.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public static class DbContextBulkExtensions
    {
        #region Insert

        public static async Task BulkInsertAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsertAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkInsert<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, entities, bulkConfig, progress, type);
        }

        #endregion

        #region Update

        public static async Task BulkUpdateAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkUpdateAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkUpdate<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkUpdate(context, entities, bulkConfig, progress, type);
        }

#pragma warning disable CA1002 // Do not expose generic lists
        public static async Task<int> BatchUpdateAsync<T>(this IQueryable<T> query, object updateValues, List<string>? updateColumns = null, CancellationToken cancellationToken = default)
#pragma warning restore CA1002 // Do not expose generic lists
            where T : class
        {
            return await EFCore.BulkExtensions.IQueryableBatchExtensions.BatchUpdateAsync(query, updateValues, updateColumns, cancellationToken);
        }

        public static async Task<int> BatchUpdateAsync<T>(this IQueryable<T> query, Expression<Func<T, T>> updateExpression, Type? type = null)
            where T : class
        {
            return await EFCore.BulkExtensions.IQueryableBatchExtensions.BatchUpdateAsync(query, updateExpression, type);
        }

#pragma warning disable CA1002 // Do not expose generic lists
        public static int BatchUpdate<T>(this IQueryable<T> query, object updateValues, List<string>? updateColumns = null)
#pragma warning restore CA1002 // Do not expose generic lists
            where T : class
        {
            return EFCore.BulkExtensions.IQueryableBatchExtensions.BatchUpdate(query, updateValues, updateColumns);
        }

        #endregion

        #region Delete

        public static async Task BulkDeleteAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkDeleteAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkDelete<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkDelete(context, entities, bulkConfig, progress, type);
        }

        public static async Task<int> BatchDeleteAsync(this IQueryable query, CancellationToken cancellationToken = default)
        {
            return await EFCore.BulkExtensions.IQueryableBatchExtensions.BatchDeleteAsync(query, cancellationToken);
        }

        public static int BatchDelete(this IQueryable query)
        {
            return EFCore.BulkExtensions.IQueryableBatchExtensions.BatchDelete(query);
        }

        #endregion

        #region Read

        public static async Task BulkReadAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkReadAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkRead<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkRead(context, entities, bulkConfig, progress, type);
        }

        #endregion

        public static async Task BulkInsertOrUpdateAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsertOrUpdateAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkInsertOrUpdate<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsertOrUpdate(context, entities, bulkConfig, progress, type);
        }

        public static async Task BulkInsertOrUpdateOrDeleteAsync<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
            where TContext : DbContext
            where TEntity : class
        {
            await EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsertOrUpdateOrDeleteAsync(context, entities, bulkConfig, progress, type, cancellationToken);
        }

        public static void BulkInsertOrUpdateOrDelete<TContext, TEntity>(this TContext context, IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null)
            where TContext : DbContext
            where TEntity : class
        {
            EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsertOrUpdateOrDelete(context, entities, bulkConfig, progress, type);
        }
    }
}

namespace Farsica.Framework.DataAccess.Bulk
{
    using System;
    using System.Collections.Generic;
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

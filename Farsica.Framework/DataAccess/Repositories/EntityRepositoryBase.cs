namespace Farsica.Framework.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EFCore.BulkExtensions;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.Query;
    using Farsica.Framework.DataAccess.Specification.Queryable;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class EntityRepositoryBase<TContext, TEntity, TKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        : RepositoryBase<TContext>, IRepository<TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TEntity, TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly OrderBy<TEntity> defaultOrderBy = new(t => t.OrderBy(e => e.Id));

        protected EntityRepositoryBase(ILogger<DataAccess> logger, TContext context)
            : base(logger, context)
        {
        }

        public IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return QueryDb(predicate, orderBy, includes, tracking);
        }

        public IQueryable<TEntity> GetManyQueryable([NotNull] ISpecification<TEntity> specification, bool tracking = false)
        {
            var query = QueryDb(specification.Expression(), specification.Order, null, tracking);
            if (specification.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.PageSize * (specification.PageFilter.CurrentPage - 1)).Take(specification.PageFilter.PageSize);
            }

            return query;
        }

        public IEnumerable<TEntity>? GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return QueryDb(null, orderBy, includes, tracking).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>?> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return await QueryDb(null, orderBy, includes, tracking).ToListAsync();
        }

        public IEnumerable<TEntity>? GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return QueryDb(null, orderBy, includes, tracking).Skip(startRow).Take(pageLength).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>?> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return await QueryDb(null, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToListAsync();
        }

        public TEntity? Get(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            return QueryDb(t => t.Id.Equals(id), null, includes, tracking).FirstOrDefault();
        }

        public TEntity? Get([NotNull] ISpecification<TEntity> specification, bool tracking = true)
        {
            return QueryDb(specification.Expression(), specification.Order, null, tracking).FirstOrDefault();
        }

        public async Task<TEntity?> GetAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            return await QueryDb(t => t.Id.Equals(id), null, includes, tracking).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetAsync([NotNull] ISpecification<TEntity> specification, bool tracking = true)
        {
            return await QueryDb(specification.Expression(), specification.Order, null, tracking).FirstOrDefaultAsync();
        }

        public IEnumerable<TEntity>? Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return QueryDb(predicate, orderBy, includes, tracking).AsEnumerable();
        }

        public IEnumerable<TEntity>? Query([NotNull] ISpecification<TEntity> specification, bool tracking = false)
        {
            var query = QueryDb(specification.Expression(), specification.Order, null, tracking);
            if (specification.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.PageSize * (specification.PageFilter.CurrentPage - 1)).Take(specification.PageFilter.PageSize);
            }

            return query.AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>?> QueryAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return await QueryDb(predicate, orderBy, includes, tracking).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>?> QueryAsync([NotNull] ISpecification<TEntity> specification, bool tracking = false)
        {
            var query = QueryDb(specification.Expression(), specification.Order, null, tracking);
            if (specification.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.PageSize * (specification.PageFilter.CurrentPage - 1)).Take(specification.PageFilter.PageSize);
            }

            return await query.ToListAsync();
        }

        public IEnumerable<TEntity>? QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return QueryDb(predicate, orderBy, includes, tracking).Skip(startRow).Take(pageLength).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>?> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return await QueryDb(predicate, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Add(entity);
        }

        public async Task BulkInsertAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
        {
            await Context.BulkInsertAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public TEntity? Update(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Context.Set<TEntity>().Update(entity).Entity;
        }

        public async Task BulkUpdateAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
        {
            await Context.BulkUpdateAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public void Remove(TKey id)
        {
            var entity = new TEntity() { Id = id };
            Remove(entity);
        }

        public void Remove(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Set<TEntity>().Remove(entity);
        }

        public async Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
        {
            await Context.BulkDeleteAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public bool Any(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return QueryDb(predicate, null, null, false).Any();
        }

        public bool Any([NotNull] ISpecification<TEntity> specification)
        {
            return QueryDb(specification.Expression(), null, null, false).Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await QueryDb(predicate, null, null, false).AnyAsync();
        }

        public async Task<bool> AnyAsync([NotNull] ISpecification<TEntity> specification)
        {
            return await QueryDb(specification.Expression(), null, null, false).AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return QueryDb(predicate, null, null, false).Count();
        }

        public int Count([NotNull] ISpecification<TEntity> specification)
        {
            return QueryDb(specification.Expression(), null, null, false).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await QueryDb(predicate, null, null, false).CountAsync();
        }

        public async Task<int> CountAsync([NotNull] ISpecification<TEntity> specification)
        {
            return await QueryDb(specification.Expression(), null, null, false).CountAsync();
        }

        public void SetUnchanged(TEntity entitieit)
        {
            Context.Entry(entitieit).State = EntityState.Unchanged;
        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes, bool tracking)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (includes is not null)
            {
                query = includes(query);
            }

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}

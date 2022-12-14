namespace Farsica.Framework.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EFCore.BulkExtensions;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.Query;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class EntityRepositoryBase<TContext, TEntity, TKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        : RepositoryBase<TContext>, IRepository<TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TEntity, TKey>, new()
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

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            var result = QueryDb(null, orderBy, includes, tracking);
            return result.AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            var result = QueryDb(null, orderBy, includes, tracking);
            return await result.ToListAsync();
        }

        public IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes, tracking);
            return result.Skip(startRow).Take(pageLength).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes, tracking);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public TEntity Get(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            if (includes != null)
            {
                query = includes(query);
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault(t => t.Id.Equals(id));
        }

        public async Task<TEntity> GetAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            var result = QueryDb(predicate, orderBy, includes, tracking);
            return result.AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            var result = QueryDb(predicate, orderBy, includes, tracking);
            return await result.ToListAsync();
        }

        public IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;

            var result = QueryDb(predicate, orderBy, includes, tracking);
            return result.Skip(startRow).Take(pageLength).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;

            var result = QueryDb(predicate, orderBy, includes, tracking);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Add(entity);
        }

        public async Task BulkInsertAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
        {
            await Context.BulkInsertAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
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
            if (entity == null)
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

        public bool Any(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public void SetUnchanged(TEntity entitieit)
        {
            Context.Entry(entitieit).State = EntityState.Unchanged;
        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes, bool tracking)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
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

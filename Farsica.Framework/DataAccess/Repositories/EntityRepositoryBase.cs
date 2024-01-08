﻿namespace Farsica.Framework.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EFCore.BulkExtensions;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.Query;
    using Farsica.Framework.DataAccess.Specification;
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

        public IQueryable<TEntity> GetManyQueryable(ISpecification<TEntity>? specification, bool tracking = false)
        {
            var query = QueryDb(specification?.Expression(), specification?.Order, null, tracking);
            if (specification?.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.Skip).Take(specification.PageFilter.Size);
            }

            return query;
        }

        public ICollection<TEntity>? GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return QueryDb(null, orderBy, includes, tracking).ToList();
        }

        public async Task<ICollection<TEntity>?> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return await QueryDb(null, orderBy, includes, tracking).ToListAsync();
        }

        public ICollection<TEntity>? GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return QueryDb(null, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToList();
        }

        public async Task<ICollection<TEntity>?> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return await QueryDb(null, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToListAsync();
        }

        public TEntity? Get(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            return QueryDb(t => t.Id.Equals(id), null, includes, tracking).FirstOrDefault();
        }

        public TEntity? Get(ISpecification<TEntity>? specification, bool tracking = true)
        {
            return QueryDb(specification?.Expression(), specification?.Order, null, tracking).FirstOrDefault();
        }

        public async Task<TEntity?> GetAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = true)
        {
            return await QueryDb(t => t.Id.Equals(id), null, includes, tracking).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecification<TEntity>? specification, bool tracking = true)
        {
            return await QueryDb(specification?.Expression(), specification?.Order, null, tracking).FirstOrDefaultAsync();
        }

        public ICollection<TEntity>? Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return QueryDb(predicate, orderBy, includes, tracking).ToList();
        }

        public ICollection<TEntity>? Query(ISpecification<TEntity>? specification, bool tracking = false)
        {
            var query = QueryDb(specification?.Expression(), specification?.Order, null, tracking);
            if (specification?.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.Skip).Take(specification.PageFilter.Size);
            }

            return query.ToList();
        }

        public async Task<ICollection<TEntity>?> QueryAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            return await QueryDb(predicate, orderBy, includes, tracking).ToListAsync();
        }

        public async Task<ICollection<TEntity>?> QueryAsync(ISpecification<TEntity>? specification, bool tracking = false)
        {
            var query = QueryDb(specification?.Expression(), specification?.Order, null, tracking);
            if (specification?.PageFilter is not null)
            {
                query = query.Skip(specification.PageFilter.Skip).Take(specification.PageFilter.Size);
            }

            return await query.ToListAsync();
        }

        public ICollection<TEntity>? QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return QueryDb(predicate, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToList();
        }

        public async Task<ICollection<TEntity>?> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null, bool tracking = false)
        {
            orderBy ??= defaultOrderBy.Expression;
            return await QueryDb(predicate, orderBy, includes, tracking).Skip(startRow).Take(pageLength).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            Context.Set<TEntity>().Add(entity);
        }

        public async Task BulkInsertAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default)
        {
            await Context.BulkInsertAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public TEntity? Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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

        public bool Any(ISpecification<TEntity>? specification)
        {
            return QueryDb(specification?.Expression(), null, null, false).Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await QueryDb(predicate, null, null, false).AnyAsync();
        }

        public async Task<bool> AnyAsync(ISpecification<TEntity>? specification)
        {
            return await QueryDb(specification?.Expression(), null, null, false).AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return QueryDb(predicate, null, null, false).Count();
        }

        public int Count(ISpecification<TEntity>? specification)
        {
            return QueryDb(specification?.Expression(), null, null, false).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await QueryDb(predicate, null, null, false).CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity>? specification)
        {
            return await QueryDb(specification?.Expression(), null, null, false).CountAsync();
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

namespace Farsica.Framework.DataAccess.Paging
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.Query;

    [DataAnnotation.Injectable]
    public interface IDataPager<TEntity, TKey>
        where TEntity : class, IEntity<TEntity, TKey>
    {
        DataPage<TEntity> Get(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        DataPage<TEntity> Query(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<DataPage<TEntity>> GetAsync(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<DataPage<TEntity>> QueryAsync(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);
    }
}

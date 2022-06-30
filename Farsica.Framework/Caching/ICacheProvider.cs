namespace Farsica.Framework.Caching
{
    using System;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.Extensions.Caching.Memory;

    [Injectable]
    public interface ICacheProvider
    {
        Task<TItem> GetAsync<TItem, TKey>(TKey key, Func<ICacheEntry, Task<TItem>> factory = null, string tenant = null)
            where TKey : struct;

        Task<TItem> GetAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory = null, string tenant = null);

        TItem Get<TItem, TKey>(TKey key, Func<ICacheEntry, TItem> factory = null, string tenant = null)
            where TKey : struct;

        TItem Get<TItem>(string key, Func<ICacheEntry, TItem> factory, string tenant = null);

        Task RemoveAsync<TKey>(TKey key, string tenant = null)
            where TKey : struct;

        Task RemoveAsync(string key, string tenant = null);

        void Remove<TKey>(TKey key, string tenant = null)
            where TKey : struct;

        void Remove(string key, string tenant = null);

        Task<TItem> SetAsync<TItem, TKey>(TKey key, TItem value, string tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions options = null)
            where TKey : struct;

        Task<TItem> SetAsync<TItem>(string key, TItem value, string tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions options = null);

        TItem Set<TItem, TKey>(TKey key, TItem value, string tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions options = null)
            where TKey : struct;

        TItem Set<TItem>(string key, TItem value, string tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions options = null);
    }
}

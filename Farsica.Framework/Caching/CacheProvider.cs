namespace Farsica.Framework.Caching
{
    using System;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.Extensions.Caching.Memory;

    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;

        public CacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<TItem> GetAsync<TItem, TKey>(TKey key, Func<ICacheEntry, Task<TItem>>? factory = null, string? tenant = null)
            where TKey : struct
        {
            return await GetAsync(key.ToString(), factory, tenant);
        }

        public async Task<TItem> GetAsync<TItem>(string? key, Func<ICacheEntry, Task<TItem>>? factory = null, string? tenant = null)
        {
            return factory == null ?
                memoryCache.Get<TItem>($"{tenant}_{key}")
                : await memoryCache.GetOrCreateAsync(GenerateKey(key, tenant), factory);
        }

        public TItem Get<TItem, TKey>(TKey key, Func<ICacheEntry, TItem>? factory = null, string? tenant = null)
            where TKey : struct
        {
            return Get(key.ToString(), factory, tenant);
        }

        public TItem Get<TItem>(string? key, Func<ICacheEntry, TItem>? factory, string? tenant = null)
        {
            return factory == null ?
                memoryCache.Get<TItem>(GenerateKey(key, tenant))
                : memoryCache.GetOrCreate(GenerateKey(key, tenant), factory);
        }

        public async Task RemoveAsync<TKey>(TKey key, string? tenant = null)
            where TKey : struct
        {
            await RemoveAsync(key.ToString(), tenant);
        }

        public async Task RemoveAsync(string? key, string? tenant = null)
        {
            await Task.Run(() => Remove(key, tenant));
        }

        public void Remove<TKey>(TKey key, string? tenant = null)
            where TKey : struct
        {
            Remove(key.ToString(), tenant);
        }

        public void Remove(string? key, string? tenant = null)
        {
            memoryCache.Remove(GenerateKey(key, tenant));
        }

        public async Task<TItem> SetAsync<TItem, TKey>(TKey key, TItem value, string? tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions? options = null)
            where TKey : struct
        {
            return await SetAsync(key.ToString(), value, tenant, slidingExpiration, options);
        }

        public async Task<TItem> SetAsync<TItem>(string? key, TItem value, string? tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions? options = null)
        {
            return await Task.FromResult(Set(key, value, tenant, slidingExpiration, options));
        }

        public TItem Set<TItem, TKey>(TKey key, TItem value, string? tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions? options = null)
            where TKey : struct
        {
            return Set(key.ToString(), value, tenant, slidingExpiration, options);
        }

        public TItem Set<TItem>(string? key, TItem value, string? tenant = null, TimeSpan? slidingExpiration = null, MemoryCacheEntryOptions? options = null)
        {
            if (options != null)
            {
                if (slidingExpiration.HasValue)
                {
                    options.SlidingExpiration = slidingExpiration;
                }

                return memoryCache.Set(GenerateKey(key, tenant), value, options);
            }

            if (slidingExpiration.HasValue)
            {
                return memoryCache.Set(GenerateKey(key, tenant), value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
            }

            return memoryCache.Set(GenerateKey(key, tenant), value);
        }

        private static string? GenerateKey(string? key, string? tenant = null)
        {
            return $"{tenant ?? string.Empty}_{key}";
        }
    }
}

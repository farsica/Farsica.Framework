namespace Farsica.Framework.Caching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.Extensions.Caching.Distributed;

    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache cache;

        public CacheProvider(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<TItem?> GetAsync<TItem, TEnum, TKey>(TEnum key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            return await GetAsync(key.Name, factory, options, tenant);
        }

        public async Task<TItem?> GetAsync<TItem, TEnum>(TEnum key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : struct
        {
            return await GetAsync(key.ToString(), factory, options, tenant);
        }

        public async Task<TItem?> GetAsync<TItem>(string? key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
        {
            var cacheKey = GenerateKey(key, tenant);
            var tmp = await cache.GetAsync(cacheKey);
            if (tmp is null)
            {
                if (factory is null)
                {
                    return default;
                }

                options ??= new DistributedCacheEntryOptions();

                var result = await factory();
                await cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(result), options);

                return result;
            }

            using var stream = new MemoryStream(tmp);
            return await JsonSerializer.DeserializeAsync<TItem?>(stream);
        }

        public TItem? Get<TItem, TEnum, TKey>(TEnum key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            return Get(key.Name, factory, options, tenant);
        }

        public TItem? Get<TItem, TKey>(TKey key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct
        {
            return Get(key.ToString(), factory, options, tenant);
        }

        public TItem? Get<TItem>(string? key, Func<TItem?>? factory, DistributedCacheEntryOptions? options = null, string? tenant = null)
        {
            var cacheKey = GenerateKey(key, tenant);
            var tmp = cache.Get(cacheKey);
            if (tmp is null)
            {
                if (factory is null)
                {
                    return default;
                }

                options ??= new DistributedCacheEntryOptions();

                var result = factory();
                cache.Set(cacheKey, JsonSerializer.SerializeToUtf8Bytes(result), options);

                return result;
            }

            using var stream = new MemoryStream(tmp);
            return JsonSerializer.Deserialize<TItem?>(stream);
        }

        public async Task RemoveAsync<TEnum, TKey>(TEnum key, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            await RemoveAsync(key.Name, tenant);
        }

        public async Task RemoveAsync<TKey>(TKey key, string? tenant = null)
            where TKey : struct
        {
            await RemoveAsync(key.ToString(), tenant);
        }

        public async Task RemoveAsync([NotNull] string key, string? tenant = null)
        {
            await cache.RemoveAsync(GenerateKey(key, tenant));
        }

        public void Remove<TEnum, TKey>(TEnum key, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            Remove(key.Name, tenant);
        }

        public void Remove<TKey>(TKey key, string? tenant = null)
            where TKey : struct
        {
            Remove(key.ToString(), tenant);
        }

        public void Remove(string? key, string? tenant = null)
        {
            cache.Remove(GenerateKey(key, tenant));
        }

        public async Task SetAsync<TItem, TEnum, TKey>(TEnum key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            await SetAsync(key.Name, value, options, tenant);
        }

        public async Task SetAsync<TItem, TKey>(TKey key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct
        {
            await SetAsync(key.ToString(), value, options, tenant);
        }

        public async Task SetAsync<TItem>([NotNull] string key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
        {
            options ??= new DistributedCacheEntryOptions();

            await cache.SetAsync(GenerateKey(key, tenant), JsonSerializer.SerializeToUtf8Bytes(value), options);
        }

        public void Set<TItem, TEnum, TKey>(TEnum key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            Set(key.Name, value, options, tenant);
        }

        public void Set<TItem, TKey>(TKey key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct
        {
            Set(key.ToString(), value, options, tenant);
        }

        public void Set<TItem>([NotNull] string key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
        {
            options ??= new DistributedCacheEntryOptions();

            cache.Set(GenerateKey(key, tenant), JsonSerializer.SerializeToUtf8Bytes(value), options);
        }

        private static string GenerateKey([NotNull] string key, string? tenant = null)
        {
            return $"{tenant ?? string.Empty}_{key}";
        }
    }
}

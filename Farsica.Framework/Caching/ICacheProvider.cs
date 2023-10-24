namespace Farsica.Framework.Caching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.Extensions.Caching.Distributed;

    [Injectable]
    public interface ICacheProvider
    {
        Task<TItem?> GetAsync<TItem, TEnum, TKey>(TEnum key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        Task<TItem?> GetAsync<TItem, TKey>(TKey key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct;

        Task<TItem?> GetAsync<TItem>(string? key, Func<Task<TItem?>>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null);

        Task<TItem?> GetAsync<TItem, TEnum, TKey>(TEnum key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        Task<TItem?> GetAsync<TItem, TKey>(TKey key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct;

        Task<TItem?> GetAsync<TItem>(string key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null);

        public TItem? Get<TItem, TEnum, TKey>(TEnum key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        public TItem? Get<TItem, TKey>(TKey key, Func<TItem?>? factory = null, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct;

        public TItem? Get<TItem>(string? key, Func<TItem?>? factory, DistributedCacheEntryOptions? options = null, string? tenant = null);

        Task RemoveAsync<TEnum, TKey>(TEnum key, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        Task RemoveAsync<TKey>(TKey key, string? tenant = null)
            where TKey : struct;

        Task RemoveAsync([NotNull] string key, string? tenant = null);

        public void Remove<TEnum, TKey>(TEnum key, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        public void Remove<TKey>(TKey key, string? tenant = null)
            where TKey : struct;

        public void Remove(string? key, string? tenant = null);

        Task SetAsync<TItem, TEnum, TKey>(TEnum key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        Task SetAsync<TItem, TKey>(TKey key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct;

        Task SetAsync<TItem>([NotNull] string key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null);

        public void Set<TItem, TEnum, TKey>(TEnum key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TEnum : Enumeration<TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>;

        public void Set<TItem, TKey>(TKey key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null)
            where TKey : struct;

        public void Set<TItem>([NotNull] string key, TItem? value, DistributedCacheEntryOptions? options = null, string? tenant = null);
    }
}

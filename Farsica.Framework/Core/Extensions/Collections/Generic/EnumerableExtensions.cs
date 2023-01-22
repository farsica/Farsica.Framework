namespace Farsica.Framework.Core.Extensions.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Farsica.Framework.Core.Extensions.Linq;
    using Farsica.Framework.Data;

    public static class EnumerableExtensions
    {
        public static string? JoinAsString(this IEnumerable<string?> source, string? separator)
        {
            return string.Join(separator, source);
        }

        public static string? JoinAsString<T>(this IEnumerable<T> source, string? separator)
        {
            return string.Join(separator, source);
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        public static async Task<(IEnumerable<TSource> List, int? TotalRecordsCount)> FilterListAsync<TSource>(this IEnumerable<TSource> lst, PagingDto pagingDto)
        {
            return await QueryableExtensions.FilterListAsync(lst.AsQueryable(), pagingDto);
        }
    }
}

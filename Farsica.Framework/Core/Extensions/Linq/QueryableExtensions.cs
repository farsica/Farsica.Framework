namespace Farsica.Framework.Core.Extensions.Linq
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Farsica.Framework.Data;
    using Microsoft.EntityFrameworkCore;

    public static class QueryableExtensions
    {
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static TQueryable PageBy<T, TQueryable>([NotNull] this TQueryable query, int skipCount, int maxResultCount)
            where TQueryable : IQueryable<T>
        {
            return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
        }

        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static async Task<(IQueryable<TSource> List, int? TotalRecordsCount)> FilterListAsync<TSource>(this IQueryable<TSource> lst, PagingDto? pagingDto)
        {
            if (pagingDto is null)
            {
                return (lst, null);
            }

            var properties = typeof(TSource).GetProperties();

            if (pagingDto.SearchFilter?.Any() is true)
            {
                foreach (var item in pagingDto.SearchFilter)
                {
                    ApplyFilter(item);
                }
            }

            var sort = false;
            if (pagingDto.SortFilter?.Any() is true)
            {
                var items = pagingDto.SortFilter.Where(t => properties.Any(p => p.Name.Equals(t.Column, StringComparison.InvariantCultureIgnoreCase)));
                if (items?.Any() is true)
                {
                    sort = true;
                    lst = lst.OrderBy(string.Join(",", items.Select(t => $"{t.Column} {t.SortType.ToString().ToLower()}")));
                }
            }

            int? total = null;

            if (pagingDto.PageFilter is not null)
            {
                if (pagingDto.PageFilter.ReturnTotalRecordsCount)
                {
                    total = lst is IEnumerable ? lst.Count() : await lst.CountAsync();
                }

                if (sort is false)
                {
                    var id = properties.FirstOrDefault(t => t.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))?.Name ?? properties.FirstOrDefault()?.Name;
                    lst = lst.OrderBy(id);
                }

                lst = lst.Skip(pagingDto.PageFilter.Skip).Take(pagingDto.PageFilter.Size);
            }

            return (lst, total);

            void ApplyFilter(SearchFilter searchFilter)
            {
                var property = properties.FirstOrDefault(t => t.Name.Equals(searchFilter.Column, StringComparison.InvariantCultureIgnoreCase));
                if (property is null)
                {
                    return;
                }

                var type = property.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var tmp = Nullable.GetUnderlyingType(type);
                    if (tmp is not null)
                    {
                        type = tmp;
                    }
                }

                if (type.IsEnum)
                {
                    var fields = type.GetFields();
                    var match = false;
                    foreach (var info in fields)
                    {
                        if (info.Name.Equals(searchFilter.Phrase, StringComparison.InvariantCultureIgnoreCase))
                        {
                            lst = lst.Where($"{property.Name} == (@0)", info.GetValue(info));
                            match = true;
                            break;
                        }
                    }

                    if (!match)
                    {
                        lst = lst.Where($"{property.Name} == (@0)", searchFilter.Phrase);
                    }
                }
                else
                {
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                            {
                                if (bool.TryParse(searchFilter.Phrase, out bool result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                            {
                                if (byte.TryParse(searchFilter.Phrase, out byte result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                            {
                                if (short.TryParse(searchFilter.Phrase, out short result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                            {
                                if (int.TryParse(searchFilter.Phrase, out int result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            {
                                if (long.TryParse(searchFilter.Phrase, out long result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Single:
                            {
                                if (float.TryParse(searchFilter.Phrase, out float result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Double:
                            {
                                if (double.TryParse(searchFilter.Phrase, out double result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.Decimal:
                            {
                                if (decimal.TryParse(searchFilter.Phrase, out decimal result))
                                {
                                    lst = lst.Where($"{property.Name} == (@0)", result);
                                }
                            }

                            break;
                        case TypeCode.DateTime:
                            lst = lst.Where($"{property.Name} == (@0)", searchFilter.Phrase);
                            break;
                        case TypeCode.String:
                        case TypeCode.Char:
                            lst = lst.Where($"{property.Name}.Contains(@0)", searchFilter.Phrase);
                            break;
                    }
                }
            }
        }
    }
}

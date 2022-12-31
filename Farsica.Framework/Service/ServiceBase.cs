namespace Farsica.Framework.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;
    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class ServiceBase<T>
        where T : class
    {
        protected ServiceBase(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Localizer = localizer;
        }

        protected Lazy<ILogger<T>> Logger { get; }

        protected Lazy<IHttpContextAccessor> HttpContextAccessor { get; }

        protected Lazy<IStringLocalizer<T>> Localizer { get; }

        protected Lazy<IUnitOfWorkProvider> UnitOfWorkProvider { get; }

        protected IEnumerable<TSource> FilterList<TSource>(PagingDto pagingDto, IEnumerable<TSource> lst, IEnumerable<(string Column, bool Descending)> orderBy)
        {
            try
            {
                if (pagingDto is null)
                {
                    return lst;
                }

                if (pagingDto.SearchFilter?.Column is not null)
                {
                    var type = typeof(TSource).GetProperty(pagingDto.SearchFilter.Column)?.PropertyType;
                    if (type is not null)
                    {
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
                                var names = new List<string?> { info.Name };

                                var customAttribute = info.GetCustomAttribute<DisplayAttribute>(false);
                                if (customAttribute is not null)
                                {
                                    names.Add(Globals.GetLocalizedValueInternal(customAttribute, info.Name, Constants.ResourceKey.Name));
                                }

                                if (names.Any(t => t is not null && t.Equals(pagingDto.SearchFilter.Phrase, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", info.GetValue(info));
                                    match = true;
                                    break;
                                }
                            }

                            if (!match)
                            {
                                lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase);
                            }
                        }
                        else if (type == typeof(LocalizableString))
                        {
                            lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column}.Value.Contains(@0)", pagingDto.SearchFilter.Phrase);
                        }
                        else
                        {
                            switch (Type.GetTypeCode(type))
                            {
                                case TypeCode.Boolean:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<bool>());
                                    break;
                                case TypeCode.SByte:
                                case TypeCode.Byte:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<byte>());
                                    break;
                                case TypeCode.Int16:
                                case TypeCode.UInt16:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<short>());
                                    break;
                                case TypeCode.Int32:
                                case TypeCode.UInt32:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<int>());
                                    break;
                                case TypeCode.Int64:
                                case TypeCode.UInt64:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<long>());
                                    break;
                                case TypeCode.Single:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<float>());
                                    break;
                                case TypeCode.Double:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<double>());
                                    break;
                                case TypeCode.Decimal:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<decimal>());
                                    break;
                                case TypeCode.DateTime:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase);
                                    break;
                                case TypeCode.String:
                                case TypeCode.Char:
                                    lst = lst.AsQueryable().Where($"{pagingDto.SearchFilter.Column}.Contains(@0)", pagingDto.SearchFilter.Phrase);
                                    break;
                            }
                        }
                    }
                }

                if (pagingDto.SortFilter is not null)
                {
                    lst = lst.AsQueryable().OrderBy($"{pagingDto.SortFilter.Column} {(pagingDto.SortFilter.Descending ? "descending" : string.Empty)}");
                }
                else if (orderBy is not null && orderBy.Any())
                {
                    lst = lst.AsQueryable().OrderBy(string.Join(",", orderBy.Select(t => $"{t.Column} {(t.Descending ? "descending" : string.Empty)}")));
                }

                return lst.Skip(pagingDto.PageSize * pagingDto.CurrentPage).Take(pagingDto.PageSize);
            }
            catch (Exception exc)
            {
                Logger.Value.LogError(exc, string.Empty);
                return lst;
            }
        }

        protected IQueryable<TSource> FilterList<TSource>(PagingDto pagingDto, IQueryable<TSource> lst, IEnumerable<(string Column, bool Descending)> orderBy)
        {
            try
            {
                if (pagingDto is null)
                {
                    return lst;
                }

                if (pagingDto.SearchFilter?.Column is not null)
                {
                    var type = typeof(TSource).GetProperty(pagingDto.SearchFilter.Column)?.PropertyType;
                    if (type is not null)
                    {
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
                                var names = new List<string> { info.Name };

                                var customAttribute = info.GetCustomAttribute<DisplayAttribute>(false);
                                if (customAttribute is not null)
                                {
                                    names.Add(Globals.GetLocalizedValueInternal(customAttribute, info.Name, Constants.ResourceKey.Name));
                                }

                                if (names.Any(t => t.Equals(pagingDto.SearchFilter.Phrase, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", info.GetValue(info));
                                    match = true;
                                    break;
                                }
                            }

                            if (!match)
                            {
                                lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase);
                            }
                        }
                        else if (type == typeof(LocalizableString))
                        {
                            lst = lst.Where($"{pagingDto.SearchFilter.Column}.Value.Contains(@0)", pagingDto.SearchFilter.Phrase);
                        }
                        else
                        {
                            switch (Type.GetTypeCode(type))
                            {
                                case TypeCode.Boolean:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<bool>());
                                    break;
                                case TypeCode.SByte:
                                case TypeCode.Byte:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<byte>());
                                    break;
                                case TypeCode.Int16:
                                case TypeCode.UInt16:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<short>());
                                    break;
                                case TypeCode.Int32:
                                case TypeCode.UInt32:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<int>());
                                    break;
                                case TypeCode.Int64:
                                case TypeCode.UInt64:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<long>());
                                    break;
                                case TypeCode.Single:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<float>());
                                    break;
                                case TypeCode.Double:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<double>());
                                    break;
                                case TypeCode.Decimal:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase.ValueOf<decimal>());
                                    break;
                                case TypeCode.DateTime:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column} == (@0)", pagingDto.SearchFilter.Phrase);
                                    break;
                                case TypeCode.String:
                                case TypeCode.Char:
                                    lst = lst.Where($"{pagingDto.SearchFilter.Column}.Contains(@0)", pagingDto.SearchFilter.Phrase);
                                    break;
                            }
                        }
                    }
                }

                if (pagingDto.SortFilter is not null)
                {
                    lst = lst.OrderBy($"{pagingDto.SortFilter.Column} {(pagingDto.SortFilter.Descending ? "descending" : string.Empty)}");
                }
                else if (orderBy is not null && orderBy.Any())
                {
                    lst = lst.OrderBy(string.Join(",", orderBy.Select(t => $"{t.Column} {(t.Descending ? "descending" : string.Empty)}")));
                }

                return lst.Skip(pagingDto.PageSize * pagingDto.CurrentPage).Take(pagingDto.PageSize);
            }
            catch (Exception exc)
            {
                Logger.Value.LogError(exc, string.Empty);
                return lst;
            }
        }
    }
}

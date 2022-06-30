namespace Farsica.Framework.Core.Utils.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using Farsica.Framework.Converter;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;
    using Farsica.Framework.Service.Factory;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;

    [Injectable]
    public abstract class ExportBase : IProvider<Constants.ExportType>
    {
        protected const string Key = "F@raB00m";

        public abstract Constants.ExportType ProviderType { get; }

        protected abstract string Extension { get; }

        protected abstract string ContentType { get; }

        protected GridDataSource GridDataSource { get; private set; }

        protected ISearch Search { get; private set; }

        public FileContentResult Export(GridDataSource gridDataSource, ISearch search, string actionName = null)
        {
            GridDataSource = gridDataSource;
            Search = search;

            var hasSearchItem = search != null;
            DataSet ds = new();
            DataTable dt = new();
            if (gridDataSource.Data?.Count > 0)
            {
                var localizedSearchColumnNames = new Dictionary<string, string>();
                var localizedGridColumnNames = new Dictionary<string, string>();
                bool dataIsDictionaryBase = (gridDataSource.Data[0] as Dictionary<string, string>) != null;
                IEnumerable<PropertyInfo> properties = null;
                if (hasSearchItem)
                {
                    var dtSearch = new DataTable();
                    var searchItemType = search.GetType();
                    properties = searchItemType.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>() == null && (t.GetCustomAttribute<ExportInfoAttribute>() == null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));
                    foreach (var info in properties)
                    {
                        var description = Globals.GetLocalizedDescription(info);
                        var name = description != null && description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                        var columnType = GetNullableType(info.PropertyType);
                        if (columnType.IsGenericType && columnType.GenericTypeArguments?.FirstOrDefault()?.Name == nameof(ParameterDto))
                        {
                            columnType = typeof(string);
                        }

                        if (columnType == typeof(DateTimeOffset) || columnType == typeof(DateTimeOffset?)
                            || columnType == typeof(DateTime) || columnType == typeof(DateTime?))
                        {
                            columnType = typeof(string);
                        }

                        dtSearch.Columns.Add(new DataColumn(name, columnType));

                        localizedSearchColumnNames.Add(info.Name, name);
                    }

                    var row = dtSearch.NewRow();
                    foreach (var info in properties)
                    {
                        var pureValue = info.GetValue(search, null);
                        if (pureValue != null)
                        {
                            var pureValueList = string.Empty;
                            var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                            if (converter != null)
                            {
                                if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                                {
                                    pureValue = instance.Convert(pureValue);
                                }
                            }

                            var columnName = localizedSearchColumnNames[info.Name];

                            if (pureValue is IList)
                            {
                                var pureValueCount = (pureValue as IList).Count;
                                if (pureValue.GetType().IsGenericType)
                                {
                                    if (pureValue.GetType().GenericTypeArguments?.FirstOrDefault()?.Name == nameof(ParameterDto))
                                    {
                                        for (int j = 0; j < pureValueCount; j++)
                                        {
                                            pureValueList += ((pureValue as IList)[j] as dynamic).Value + (j != pureValueCount - 1 ? "," : string.Empty);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < pureValueCount; j++)
                                    {
                                        pureValueList += ((pureValue as IList)[j] as dynamic).Value + (j != pureValueCount - 1 ? "," : string.Empty);
                                    }
                                }

                                pureValue = pureValueList;
                            }

                            row[columnName] = pureValue;
                        }
                    }

                    dtSearch.Rows.Add(row);
                    ds.Tables.Add(dtSearch);
                }

                if (gridDataSource.DataTable != null)
                {
                    dataIsDictionaryBase = true;

                    foreach (DataColumn column in gridDataSource.DataTable.Columns)
                    {
                        if (column.DataType == typeof(HtmlString))
                        {
                            break;
                        }

                        localizedGridColumnNames.Add(column.ColumnName, column.Caption);
                        dt.Columns.Add(new DataColumn(column.Caption, typeof(string)));
                    }
                }
                else
                {
                    if (gridDataSource.Data[0] is Dictionary<string, string> captions)
                    {
                        foreach (string key in captions.Keys)
                        {
                            localizedGridColumnNames.Add(key, key);
                            dt.Columns.Add(new DataColumn(key, typeof(string)));
                        }
                    }
                    else
                    {
                        localizedGridColumnNames = new Dictionary<string, string>();
                        var type = gridDataSource.Data[0].GetType();
                        properties = type.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) == null && (t.GetCustomAttribute<ExportInfoAttribute>() == null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));
                        var i = 0;
                        foreach (var info in properties)
                        {
                            var description = Globals.GetLocalizedDescription(info);
                            var name = description != null && description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                            if (dt.Columns.Contains(name))
                            {
                                name += i;
                            }

                            localizedGridColumnNames.Add(info.Name, name);
                            var columnType = GetNullableType(info.PropertyType);

                            if (columnType == typeof(bool))
                            {
                                var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                if (exportInfo != null && !exportInfo.TrueResourceKey.IsNullOrEmpty())
                                {
                                    columnType = typeof(string);
                                }
                            }

                            if (columnType == typeof(DateTimeOffset) || columnType == typeof(DateTimeOffset?)
                                || columnType == typeof(DateTime) || columnType == typeof(DateTime?))
                            {
                                columnType = typeof(string);
                            }

                            dt.Columns.Add(new DataColumn(name, columnType));
                            i++;
                        }
                    }
                }

                foreach (var t in gridDataSource.Data)
                {
                    var row = dt.NewRow();

                    if (dataIsDictionaryBase)
                    {
                        var dictionaryValues = t as Dictionary<string, string>;
                        foreach (var key in dictionaryValues.Keys)
                        {
                            if (gridDataSource.DataTable.Columns[key].DataType == typeof(HtmlString))
                            {
                                break;
                            }

                            var value = dictionaryValues[key];
                            if (gridDataSource.DataTable != null)
                            {
                                if (gridDataSource.DataTable.Columns[key].ExtendedProperties[nameof(ExportInfoAttribute)] is ExportInfoAttribute exportInfo && !exportInfo.TrueResourceKey.IsNullOrEmpty())
                                {
                                    if (exportInfo.ResourceType == null)
                                    {
                                        exportInfo.ResourceType = typeof(GlobalResource);
                                    }

                                    var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                    value = value.ValueOf<bool>() ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                }
                            }

                            row[localizedGridColumnNames[key]] = value;
                        }
                    }
                    else
                    {
                        foreach (var info in properties)
                        {
                            var pureValue = info.GetValue(t, null);
                            if (pureValue != null)
                            {
                                var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                                if (converter != null)
                                {
                                    if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                                    {
                                        pureValue = instance.Convert(pureValue);
                                    }
                                }

                                var typeValue = GetNullableTypeValue(info.PropertyType);
                                if (typeValue.IsEnum)
                                {
                                    pureValue = EnumHelper.LocalizeEnum(pureValue);
                                }
                                else if (typeValue == typeof(bool))
                                {
                                    var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                    if (exportInfo != null && !exportInfo.TrueResourceKey.IsNullOrEmpty())
                                    {
                                        if (exportInfo.ResourceType is null)
                                        {
                                            exportInfo.ResourceType = typeof(GlobalResource);
                                        }

                                        var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                        pureValue = (bool)pureValue ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                    }
                                }
                            }

                            var columnName = localizedGridColumnNames[info.Name];
                            if (!IsNullableType(info.PropertyType))
                            {
                                row[columnName] = pureValue;
                            }
                            else
                            {
                                row[columnName] = pureValue ?? DBNull.Value;
                            }
                        }
                    }

                    dt.Rows.Add(row);
                }
            }

            ds.Tables.Add(dt);

            var fileDownloadName = "Report-" + actionName + "-" + DateTime.Now + Extension;
            return new FileContentResult(GenerateFile(ds, hasSearchItem), ContentType) { FileDownloadName = fileDownloadName };
        }

        protected static Type GetNullableTypeValue(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(t);
            }

            return t;
        }

        protected static Type GetNullableType(Type t)
        {
            var returnType = GetNullableTypeValue(t);
            if (returnType.IsEnum)
            {
                returnType = typeof(string);
            }

            return returnType;
        }

        protected static bool IsNullableType(Type type)
        {
            return type == typeof(string) ||
                         type.IsArray ||
                         (type.IsGenericType &&
                            type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        protected abstract byte[] GenerateFile(DataSet dataSet, bool hasSearchItem);
    }
}

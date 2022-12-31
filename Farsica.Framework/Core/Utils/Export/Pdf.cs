/*namespace Farsica.Framework.Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using Farsica.Framework.Converter;
    using Farsica.Framework.Core.Utils.Export;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Html;

    public class Pdf : ExportBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Pdf(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public override Constants.ExportType ProviderType => Constants.ExportType.Pdf;

        protected override string? Extension => ".pdf";

        protected override string? ContentType => "application/pdf";

        protected override byte[] GenerateFile(DataSet dataSet, bool hasSearchItem)
        {
            Document doc = new(PageSize.A4.Rotate());
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false;
            doc.Open();
            AddLogoToDocument(doc);
            string? font_ttf = new Uri(webHostEnvironment.WebRootPath + "\\lib\\vazir\\webfonts\\Vazir-FD.ttf").AbsolutePath;
            BaseFont basefont = BaseFont.CreateFont(font_ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(basefont, 12, iTextSharp.text.Font.NORMAL);

            IEnumerable<PropertyInfo> properties = null;
            var localizedColumnNames = new Dictionary<string, string>();
            if (hasSearchItem)
            {
                var searchItemType = Search.GetType();
                properties = searchItemType.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) is null && (t.GetCustomAttribute<ExportInfoAttribute>() is null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));

                PdfPTable table1 = new PdfPTable(properties.Count())
                {
                    TotalWidth = 770f,
                    LockedWidth = true,
                    SpacingBefore = 30f,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                foreach (var info in properties)
                {
                    var description = Globals.GetLocalizedDescription(info);
                    var name = description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                    var cell = new PdfPCell(new Phrase(name, font)) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
                    cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                    cell.HorizontalAlignment = 1;
                    table1.AddCell(cell);
                }

                foreach (var info in properties)
                {
                    var pureValue = info.GetValue(Search, null);
                    var pureValueList = string.Empty;
                    if (pureValue is not null)
                    {
                        var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                        if (converter is not null)
                        {
                            if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                            {
                                pureValue = instance.Convert(pureValue);
                            }
                        }

                        if (pureValue is IList)
                        {
                            var pureValueCount = (pureValue as IList).Count;
                            if (pureValue.GetType().IsGenericType)
                            {
                                if (pureValue.GetType().GenericTypeArguments.Any())
                                {
                                    if (pureValue.GetType().GenericTypeArguments[0].Name == nameof(ParameterDto))
                                    {
                                        for (int j = 0; j < pureValueCount; j++)
                                        {
                                            pureValueList += ((pureValue as IList)[j] as dynamic).Value + (j != pureValueCount - 1 ? "," : string.Empty);
                                        }
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

                        var rowCell = new PdfPCell(new Phrase(pureValue.ToString(), font)) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
                        rowCell.HorizontalAlignment = 1;
                        table1.AddCell(rowCell);
                    }
                    else
                    {
                        var cell = new PdfPCell();
                        table1.AddCell(cell);
                    }
                }

                table1.WriteSelectedRows(0, -1, doc.Left, 500f, writer.DirectContent);
            }

            if (GridDataSource.Data?.Count > 0)
            {
                var pagingType = GridDataSource.Data[0].GetType();
                properties = pagingType.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) is null && (t.GetCustomAttribute<ExportInfoAttribute>() is null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));

                AddTableToITextSharpDocument(GridDataSource.Data, properties, writer, doc, font, GridDataSource.DataTable);
            }

            doc.Close();
            stream.Seek(0, SeekOrigin.Begin);
            stream.Flush();
            stream.Position = 0;
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            PdfReader reader = new PdfReader(data);
            PdfEncryptor.Encrypt(reader, stream, true, Key, Key, PdfWriter.ALLOW_SCREENREADERS);
            stream.Close();

            return data;
        }

        private static PdfPTable AddTableHeader(IEnumerable<PropertyInfo> properties, iTextSharp.text.Font font)
        {
            PdfPTable tablePaging = new PdfPTable(properties.Count())
            {
                TotalWidth = 770f,
                LockedWidth = true,
                SpacingBefore = 30f,
                SplitLate = false,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
            };
            foreach (var info in properties)
            {
                var description = Globals.GetLocalizedDescription(info);
                var name = description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                var cell = new PdfPCell(new Phrase(name, font)) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                cell.HorizontalAlignment = 1;
                tablePaging.AddCell(cell);
            }

            return tablePaging;
        }

        private static PdfPTable AddDataTableHeader(DataTable dataTable, iTextSharp.text.Font font)
        {
            var htmlStringColumn = new List<string>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].DataType == typeof(HtmlString))
                {
                    htmlStringColumn.Add(dataTable.Columns[i].ColumnName);
                }
            }

            PdfPTable tablePaging = new PdfPTable(dataTable.Columns.Count - htmlStringColumn.Count)
            {
                TotalWidth = 770f,
                LockedWidth = true,
                SpacingBefore = 30f,
                SplitLate = false,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
            };
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].DataType == typeof(HtmlString))
                {
                    break;
                }

                var cell = new PdfPCell(new Phrase(dataTable.Columns[i].Caption, font)) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                cell.HorizontalAlignment = 1;
                tablePaging.AddCell(cell);
            }

            return tablePaging;
        }

        private static void AddTablePaging(object value, iTextSharp.text.Font font, PropertyInfo info, ref PdfPTable tablePaging)
        {
            iTextSharp.text.Image pic = null;
            if (info is not null)
            {
                var typeValue = GetNullableTypeValue(info.PropertyType);
                if (typeValue.IsEnum)
                {
                    value = EnumHelper.LocalizeEnum(value);
                }

                if (info.PropertyType == typeof(bool))
                {
                    value = (bool)value ? GlobalResource.Yes : GlobalResource.No;
                }
            }

            if (value.ToString().StartsWith("data:image"))
            {
                var imageData = value.ToString().Split(' ')[1];
                var imageBytes = Convert.FromBase64String(imageData);
                System.Drawing.Image data;
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    data = System.Drawing.Image.FromStream(ms);
                }

                data = new Bitmap(data, new Size(70, 40));
                pic = iTextSharp.text.Image.GetInstance(data, ImageFormat.Png);
                var picCell = new PdfPCell(pic)
                {
                    HorizontalAlignment = 1,
                };
                tablePaging.AddCell(picCell);
            }

            var cell = new PdfPCell(new Phrase(value.ToString(), font)) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
            cell.HorizontalAlignment = 1;
            tablePaging.AddCell(cell);
        }

        private void AddTableToITextSharpDocument(IList lst, IEnumerable<PropertyInfo> properties, PdfWriter writer, Document doc, iTextSharp.text.Font font, DataTable dataTable = null)
        {
            var tableRows = 1;
            if (dataTable is not null)
            {
                var tablePaging = AddDataTableHeader(dataTable, font);
                for (var i = 0; i <= lst.Count; i++)
                {
                    i = 0;
                    var dictionaryValues = lst[i] as Dictionary<string, string>;
                    foreach (var key in dictionaryValues.Keys)
                    {
                        if (dataTable.Columns[key].DataType == typeof(HtmlString))
                        {
                            break;
                        }

                        if (tableRows % 6 == 0)
                        {
                            tablePaging.WriteSelectedRows(0, 9, doc.Left, 430f, writer.DirectContent);
                            doc.NewPage();

                            AddLogoToDocument(doc);
                            tablePaging = new PdfPTable(tablePaging.AbsoluteWidths.Length);
                            tablePaging = AddDataTableHeader(dataTable, font);
                            tableRows = 1;
                        }

                        var value = dictionaryValues[key];

                        AddTablePaging(value, font, null, ref tablePaging);
                    }

                    lst.Remove(lst[i]);
                    if (lst.Count == 0)
                    {
                        tablePaging.WriteSelectedRows(0, -1, doc.Left, 430f, writer.DirectContent);
                        break;
                    }

                    tableRows++;
                }
            }
            else
            {
                var tablePaging = AddTableHeader(properties, font);
                for (var i = 0; i <= lst.Count; i++)
                {
                    i = 0;
                    foreach (var info in properties)
                    {
                        if (tableRows % 6 == 0)
                        {
                            tablePaging.WriteSelectedRows(0, 9, doc.Left, 430f, writer.DirectContent);
                            doc.NewPage();

                            AddLogoToDocument(doc);
                            tablePaging = new PdfPTable(properties.Count());
                            tablePaging = AddTableHeader(properties, font);
                            tableRows = 1;
                        }

                        var pureValue = info.GetValue(lst[i], null);
                        if (pureValue is not null)
                        {
                            var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                            if (converter is not null)
                            {
                                if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                                {
                                    pureValue = instance.Convert(pureValue);
                                }
                            }

                            AddTablePaging(pureValue, font, info, ref tablePaging);
                        }
                        else
                        {
                            tablePaging.AddCell(new PdfPCell());
                        }
                    }

                    lst.Remove(lst[i]);
                    if (lst.Count == 0)
                    {
                        tablePaging.WriteSelectedRows(0, -1, doc.Left, 430f, writer.DirectContent);
                        break;
                    }

                    tableRows++;
                }
            }
        }

        private void AddLogoToDocument(Document doc)
        {
            string? imageUrl = new Uri(webHostEnvironment.WebRootPath + "\\img\\logo-mini.png").AbsolutePath;
            var logo = iTextSharp.text.Image.GetInstance(imageUrl);
            logo.SetAbsolutePosition(400, 520);
            logo.ScaleAbsoluteHeight(50);
            logo.ScaleAbsoluteWidth(50);

            doc.Add(logo);
        }
    }
}
*/

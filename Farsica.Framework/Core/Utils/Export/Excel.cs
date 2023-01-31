namespace Farsica.Framework.Core.Utils
{
    using System;
    using System.Data;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    using Farsica.Framework.Core.Utils.Export;
    using Farsica.Framework.Data;
    using Farsica.Framework.Resources;

    using Border = DocumentFormat.OpenXml.Spreadsheet.Border;
    using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
    using draw = DocumentFormat.OpenXml.Drawing;
    using Drawing = DocumentFormat.OpenXml.Spreadsheet.Drawing;
    using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
    using oDraw = DocumentFormat.OpenXml.Office2010.Drawing;
    using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;

    public class Excel : ExportBase
    {
        public override ExportType ProviderType => ExportType.Excel;

        protected override string? Extension => ".xlsx";

        protected override string? ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected override byte[] GenerateFile(DataSet dataSet, bool hasSearchItem)
        {
            using var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                WriteExcelFile(dataSet, document, hasSearchItem);
            }

            stream.Flush();
            stream.Position = 0;

            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            return data;
        }

        private static void WriteExcelFile(DataSet dataSet, SpreadsheetDocument spreadsheet, bool hasSearchItem)
        {
            WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();

            workbookPart.Workbook = new Workbook
            {
                WorkbookProtection = new WorkbookProtection
                {
                    LockStructure = true,
                },
            };
            workbookPart.Workbook.Append(new BookViews(new WorkbookView()));
            uint worksheetNumber = 1;

            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>("IdSheetPart" + worksheetNumber.ToString());

            newWorksheetPart.Worksheet =
                    new Worksheet(
                        new SheetViews(new SheetView { WorkbookViewId = 0, RightToLeft = Globals.IsRtl }),
                        new SheetData());

            var stylePart = workbookPart.AddNewPart<WorkbookStylesPart>("IdStyles");

            stylePart.Stylesheet = GenerateStylesheet();
            stylePart.Stylesheet.CellFormats.Count = UInt32Value.FromUInt32((uint)stylePart.Stylesheet.CellFormats.ChildElements.Count);
            stylePart.Stylesheet.Save();
            if (worksheetNumber == 1)
            {
                workbookPart.Workbook.AppendChild(new Sheets());
            }

            workbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = worksheetNumber,
                Name = "Excel Report",
            });

            uint rowIndex = 0;
            var isFirstTable = true;

            foreach (DataTable dt in dataSet.Tables)
            {
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart, hasSearchItem, isFirstTable, ref rowIndex);
                newWorksheetPart.Worksheet.Save();

                string? drawingID = "IdDrawingsPart";

                if (newWorksheetPart.DrawingsPart is not null
                && newWorksheetPart.DrawingsPart.WorksheetDrawing is not null)
                {
                    Drawing drawing1 = new Drawing() { Id = drawingID };
                    newWorksheetPart.Worksheet.Append(drawing1);
                }

                isFirstTable = false;
                worksheetNumber++;
            }

            foreach (var worksheetPart in spreadsheet.WorkbookPart.WorksheetParts)
            {
                string? hexConvertedPassword = HashPassword(Key);
                SheetProtection sheetProt = new()
                {
                    Sheet = true,
                    Objects = true,
                    FormatColumns = false,
                    Scenarios = true,
                    Password = hexConvertedPassword,
                };
                worksheetPart.Worksheet.InsertAfter(sheetProt, worksheetPart.Worksheet.Descendants<SheetData>().LastOrDefault());
                worksheetPart.Worksheet.Save();
            }

            // workbookPart.Workbook.Save();
        }

        private static string? HashPassword(string password)
        {
            byte[] passwordCharacters = System.Text.Encoding.ASCII.GetBytes(password);
            int hash = 0;
            if (passwordCharacters.Length > 0)
            {
                int charIndex = passwordCharacters.Length;

                while (charIndex-- > 0)
                {
                    hash = ((hash >> 14) & 0x01) | ((hash << 1) & 0x7fff);
                    hash ^= passwordCharacters[charIndex];
                }

                hash = ((hash >> 14) & 0x01) | ((hash << 1) & 0x7fff);
                hash ^= passwordCharacters.Length;
                hash ^= 0x8000 | ('N' << 8) | 'K';
            }

            return Convert.ToString(hash, 16).ToUpperInvariant();
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart, bool hasSearchItem, bool isFirstTable, ref uint rowIndexTemp)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var numberOfColumns = dt.Columns.Count;
            var columnRenderType = new CellValues[numberOfColumns];

            var rowIndex = hasSearchItem ? 3U : 2;
            var excelColumnNames = new string[numberOfColumns];
            if (rowIndexTemp != 0)
            {
                rowIndex = rowIndexTemp + 2;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                excelColumnNames[i] = GetExcelColumnName(rowIndex == 3 && hasSearchItem ? i + 1 : i);
            }

            var headerRow = new Row { RowIndex = rowIndex };
            sheetData.Append(headerRow);
            for (var i = 0; i < numberOfColumns; i++)
            {
                // if (i == 0 && isFirstTable) { }
                // AppendCell("B2", "logo-admin.png", headerRow, true, CellValues.String, worksheetPart, i, 2);
                var col = dt.Columns[i];
                AppendCell(excelColumnNames[i] + rowIndex.ToString(), col.ColumnName, headerRow, true, CellValues.String, worksheetPart, i, (int)rowIndex);
                switch (col.DataType.FullName)
                {
                    case "System.Decimal":
                    case "System.Int32":
                        columnRenderType[i] = CellValues.Number;
                        break;
                    case "System.Boolean":
                        columnRenderType[i] = CellValues.Boolean;
                        break;
                    default:
                        columnRenderType[i] = CellValues.String;
                        break;
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                var newExcelRow = new Row { RowIndex = ++rowIndex };
                sheetData.Append(newExcelRow);

                for (var colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    var cellValue = dr.ItemArray[colInx].ToString();
                    AppendCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow, false, columnRenderType[colInx],
                            worksheetPart, colInx, (int)rowIndex);
                }
            }

            rowIndexTemp = rowIndex;
        }

        private static void AppendCell(string cellReference, string? cellStringValue, Row excelRow, bool isHeaderRow, CellValues dataType, WorksheetPart worksheetPart, int colInx, int rowIndex)
        {
            var isImage = false;
            var appendCell = true;
            switch (dataType)
            {
                case CellValues.Boolean:
                    {
                        if (bool.TryParse(cellStringValue, out bool cellBooleanValue))
                        {
                            cellStringValue = cellBooleanValue ? GlobalResource.Yes : GlobalResource.No;
                            dataType = CellValues.String;
                        }
                        else
                        {
                            appendCell = false;
                        }
                    }

                    break;
                case CellValues.Number:
                    {
                        if (double.TryParse(cellStringValue, out double cellNumericValue))
                        {
                            cellStringValue = cellNumericValue.ToString();
                        }
                        else
                        {
                            appendCell = false;
                        }
                    }

                    break;
                case CellValues.String:
                    {
                        if (cellStringValue.StartsWith("data:image/") /*|| cellStringValue.Equals("logo-admin.png")*/)
                        {
                            isImage = true;

                            var contentType = "image/png";
                            var imageFormat = ImageFormat.Png;
                            if (cellStringValue.StartsWith("data:image/jpeg"))
                            {
                                contentType = "image/jpeg";
                                imageFormat = ImageFormat.Jpeg;
                            }

                            DrawingsPart drawingsPart = null;
                            Xdr.WorksheetDrawing worksheetDrawing = new Xdr.WorksheetDrawing();
                            if (worksheetPart.DrawingsPart is null)
                            {
                                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>("IdDrawingsPart");
                                drawingsPart.WorksheetDrawing = worksheetDrawing;
                            }
                            else if (worksheetPart.DrawingsPart is not null
                                            && worksheetPart.DrawingsPart.WorksheetDrawing is not null)
                            {
                                drawingsPart = worksheetPart.DrawingsPart;
                                worksheetDrawing = worksheetPart.DrawingsPart.WorksheetDrawing;
                            }

                            var imageId = "Id" + rowIndex;

                            Xdr.TwoCellAnchor cellAnchor = AddTwoCellAnchor(rowIndex - 1, colInx, rowIndex, colInx + 1, imageId);

                            worksheetDrawing.Append(cellAnchor);

                            byte[] imageBytes = null;

                            // if (cellStringValue.Equals("logo-admin.png"))
                            // {
                            // var request = System.Web.HttpContext.Current.Request;
                            // var appBaseUrl = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);

                            // var imageUrl = string.Format("{0}/images/Tenants/BoomMarket/{1}", appBaseUrl, cellStringValue);

                            // using (WebResponse wrFileResponse = WebRequest.Create(imageUrl).GetResponse())
                            // {
                            // using (Stream objWebStream = wrFileResponse.GetResponseStream())
                            // {
                            // MemoryStream ms = new MemoryStream();
                            // objWebStream.CopyTo(ms, 8192);
                            // imageBytes = ms.ToArray();
                            // }
                            // }
                            // }
                            // else
                            // {
                            cellStringValue = cellStringValue.Split(' ')[1];
                            imageBytes = Convert.FromBase64String(cellStringValue);

                            // }
                            using var ms = new MemoryStream(imageBytes);
                            var image = System.Drawing.Image.FromStream(ms);

                            var stream = new MemoryStream();
                            image.Save(stream, imageFormat);
                            stream.Position = 0;

                            ImagePart imagePart = drawingsPart.AddNewPart<ImagePart>(contentType, imageId);
                            imagePart.FeedData(stream);

                            excelRow.Height = image.Size.Height <= 40 ? image.Size.Height : 40;
                            excelRow.CustomHeight = true;
                        }
                    }

                    break;
            }

            if (appendCell && !isImage)
            {
                var cell = new Cell { CellReference = cellReference, DataType = dataType };
                var cellValue = new CellValue { Text = cellStringValue };
                cell.Append(cellValue);
                if (isHeaderRow)
                {
                    cell.StyleIndex = 2U;
                }
                else
                {
                    cell.StyleIndex = 1U;
                }

                excelRow.Height = 20;
                excelRow.CustomHeight = true;
                excelRow.Append(cell);
            }
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new(
                    new Font(// Index 0 - default
                            new FontSize() { Val = 10 }),
                    new Font(// Index 1 - header
                            new FontSize() { Val = 10 },
                            new Bold(),
                            new Color() { Rgb = "000000" }));

            Fills fills = new Fills(
                            new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                            new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 1 - default
                            new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "CCCCCCCC" } })
                            { PatternType = PatternValues.Solid })); // Index 2 - header

            Borders borders = new Borders(
                            new Border(), // index 0 default
                            new Border(// index 1 black border
                                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new DiagonalBorder()));

            CellFormats cellFormats = new CellFormats(
                            new CellFormat() { ApplyBorder = true }, // default
                            new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, ApplyProtection = true, Protection = new Protection() { Locked = true } }, // body
                            new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyBorder = true, ApplyFill = true, ApplyProtection = true, Protection = new Protection() { Locked = true } }); // header

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        private static Xdr.TwoCellAnchor AddTwoCellAnchor(int startRow, int startColumn, int endRow, int endColumn, string? imageId)
        {
            Xdr.TwoCellAnchor twoCellAnchor1 = new Xdr.TwoCellAnchor() { EditAs = Xdr.EditAsValues.OneCell };

            Xdr.FromMarker fromMarker1 = new Xdr.FromMarker();
            Xdr.ColumnId columnId1 = new Xdr.ColumnId
            {
                Text = startColumn.ToString(),
            };
            Xdr.ColumnOffset columnOffset1 = new Xdr.ColumnOffset
            {
                Text = "0",
            };
            Xdr.RowId rowId1 = new Xdr.RowId
            {
                Text = startRow.ToString(),
            };
            Xdr.RowOffset rowOffset1 = new Xdr.RowOffset
            {
                Text = "0",
            };

            fromMarker1.Append(columnId1);
            fromMarker1.Append(columnOffset1);
            fromMarker1.Append(rowId1);
            fromMarker1.Append(rowOffset1);

            Xdr.ToMarker toMarker1 = new Xdr.ToMarker();
            Xdr.ColumnId columnId2 = new Xdr.ColumnId
            {
                Text = endColumn.ToString(),
            };
            Xdr.ColumnOffset columnOffset2 = new Xdr.ColumnOffset
            {
                Text = "0", // "152381";
            };
            Xdr.RowId rowId2 = new Xdr.RowId
            {
                Text = endRow.ToString(),
            };
            Xdr.RowOffset rowOffset2 = new Xdr.RowOffset
            {
                Text = "0", // "152381";
            };

            toMarker1.Append(columnId2);
            toMarker1.Append(columnOffset2);
            toMarker1.Append(rowId2);
            toMarker1.Append(rowOffset2);

            Xdr.Picture picture1 = new Xdr.Picture();

            Xdr.NonVisualPictureProperties nonVisualPictureProperties1 = new Xdr.NonVisualPictureProperties();
            Xdr.NonVisualDrawingProperties nonVisualDrawingProperties1 =
                    new Xdr.NonVisualDrawingProperties() { Id = 2U, Name = "Picture 1" };

            Xdr.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties1 =
                    new Xdr.NonVisualPictureDrawingProperties();
            draw.PictureLocks pictureLocks1 = new draw.PictureLocks() { NoChangeAspect = true };

            nonVisualPictureDrawingProperties1.Append(pictureLocks1);

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            Xdr.BlipFill blipFill1 = new Xdr.BlipFill();

            draw.Blip blip1 = new draw.Blip() { Embed = imageId };
            blip1.AddNamespaceDeclaration(
                "r",
                "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            draw.BlipExtensionList blipExtensionList1 = new draw.BlipExtensionList();

            draw.BlipExtension blipExtension1 = new draw.BlipExtension()
            { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            oDraw.UseLocalDpi useLocalDpi1 = new oDraw.UseLocalDpi() { Val = false };
            useLocalDpi1.AddNamespaceDeclaration(
                "oDraw",
                "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.Append(useLocalDpi1);

            blipExtensionList1.Append(blipExtension1);

            blip1.Append(blipExtensionList1);

            draw.Stretch stretch1 = new draw.Stretch();
            draw.FillRectangle fillRectangle1 = new draw.FillRectangle();

            stretch1.Append(fillRectangle1);

            blipFill1.Append(blip1);
            blipFill1.Append(stretch1);

            Xdr.ShapeProperties shapeProperties1 = new Xdr.ShapeProperties();

            draw.Transform2D transform2D1 = new draw.Transform2D();
            draw.Offset offset1 = new draw.Offset() { X = 0L, Y = 0L };
            draw.Extents extents1 = new draw.Extents() { Cx = 152381L, Cy = 152381L };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            draw.PresetGeometry presetGeometry1 = new draw.PresetGeometry() { Preset = draw.ShapeTypeValues.Rectangle };
            draw.AdjustValueList adjustValueList1 = new draw.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);

            picture1.Append(nonVisualPictureProperties1);
            picture1.Append(blipFill1);
            picture1.Append(shapeProperties1);
            Xdr.ClientData clientData1 = new Xdr.ClientData();

            twoCellAnchor1.Append(fromMarker1);
            twoCellAnchor1.Append(toMarker1);
            twoCellAnchor1.Append(picture1);
            twoCellAnchor1.Append(clientData1);

            return twoCellAnchor1;
        }

        private static string? GetExcelColumnName(int columnIndex)
        {
            // Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return $"{firstChar}{secondChar}";
        }
    }
}

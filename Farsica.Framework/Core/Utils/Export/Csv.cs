namespace Farsica.Framework.Core.Utils
{
    using System;
    using System.Data;
    using System.Text;

    using Farsica.Framework.Core.Utils.Export;
    using Farsica.Framework.Data;

    public class Csv : ExportBase
    {
        public override ExportType ProviderType => ExportType.Csv;

        protected override string Extension => ".csv";

        protected override string ContentType => "text/csv";

        protected override byte[] GenerateFile(DataSet dataSet)
        {
            var dt = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : dataSet.Tables[0];
            StringBuilder builder = new();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                builder.Append(dt.Columns[i].ColumnName);
                builder.Append(i == dt.Columns.Count - 1 ? Environment.NewLine : '|');
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.AppendLine(string.Join("|", dt.Rows[i].ItemArray));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}

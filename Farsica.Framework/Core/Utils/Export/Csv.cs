namespace Farsica.Framework.Core.Utils
{
    using System.Data;
    using System.Text;

    using Farsica.Framework.Core.Utils.Export;

    public class Csv : ExportBase
    {
        public override Constants.ExportType ProviderType => Constants.ExportType.Csv;

        protected override string Extension => ".csv";

        protected override string ContentType => "text/csv";

        protected override byte[] GenerateFile(DataSet dataSet, bool hasSearchItem)
        {
            var dt = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : dataSet.Tables[0];
            StringBuilder builder = new();
            foreach (DataColumn item in dt.Columns)
            {
                builder.Append(item.ColumnName);
                builder.Append('|');
            }

            builder.Remove(builder.Length - 1, 1);

            foreach (DataRow row in dt.Rows)
            {
                builder.AppendLine(string.Join("|", row.ItemArray));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}

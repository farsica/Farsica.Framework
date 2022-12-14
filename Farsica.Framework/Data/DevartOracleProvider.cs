namespace Farsica.Framework.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class DevartOracleProvider : DbProviderFactory
    {
        protected override IReadOnlyDictionary<DataType, string> Mapper => new Dictionary<DataType, string>
        {
            { DataType.Boolean, "bool" },
            { DataType.Byte, "byte" },
            { DataType.Short, "Int16" },
            { DataType.Int, "Int" },
            { DataType.Long, "Int64" },
            { DataType.Decimal, "decimal" },
            { DataType.String, "varchar2" },
            { DataType.UnicodeString, "nvarchar2" },
            { DataType.MaxString, "CLOB" },
            { DataType.UnicodeMaxString, "NCLOB" },
            { DataType.Char, "CHAR" },
            { DataType.UnicodeChar, "NCHAR" },
            { DataType.Date, "DATE" },
            { DataType.Time, "TIMESTAMP" },
            { DataType.DateTime, "DATE" },
            { DataType.DateTimeOffset, "datetimeoffset" },
            { DataType.Guid, "GUID" },
            { DataType.File, "BLOB" },
        };

        protected override string? GetObjectNameInternal(string name, string? prefix = null)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                name = $"{prefix}_{name}";
            }

            var subNames = name.Split('_');
            var lst = subNames.Select(t =>
            {
                var sb = new StringBuilder();
                for (var i = 0; i < t.Length; i++)
                {
                    if (i > 0 && char.IsUpper(t[i]) && char.IsLower(t[i - 1]))
                    {
                        sb.Append('_');
                    }

                    sb.Append(t[i]);
                }

                return sb.ToString().ToUpperInvariant();
            });

            return string.Join("_", lst);
        }
    }
}

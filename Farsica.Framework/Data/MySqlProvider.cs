namespace Farsica.Framework.Data
{
    using System.Collections.Generic;

    internal class MySqlProvider : DbProviderFactory
    {
        protected override IReadOnlyDictionary<DataType, string> Mapper => new Dictionary<DataType, string>
        {
            { DataType.Boolean, "BIT" },
            { DataType.Byte, "TINYINT" },
            { DataType.Short, "SMALLINT" },
            { DataType.Int, "INT" },
            { DataType.Long, "BIGINT" },
            { DataType.Decimal, "DECIMAL" },
            { DataType.String, "VARCHAR" },
            { DataType.UnicodeString, "VARCHAR" },
            { DataType.MaxString, "LONGTEXT" },
            { DataType.UnicodeMaxString, "LONGTEXT" },
            { DataType.Char, "CHAR" },
            { DataType.UnicodeChar, "CHAR" },
            { DataType.Date, "DATE" },
            { DataType.Time, "TIME" },
            { DataType.DateTime, "DATETIME" },
            { DataType.DateTimeOffset, "datetimeoffset" },
            { DataType.Guid, "CHAR(36)" },
            { DataType.File, "LONGBLOB" },
            { DataType.Ulid, "CHAR(26)" },
        };

        protected override string GetObjectNameInternal(string name, string? prefix = null)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                name = $"{prefix}.{name}";
            }

            return name;
        }
    }
}

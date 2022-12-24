namespace Farsica.Framework.DataAnnotation.Schema
{
    using System.Diagnostics.CodeAnalysis;
    using Farsica.Framework.Data;

    public sealed class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
    {
        public ColumnAttribute([NotNull] string name)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
        }

        public ColumnAttribute([NotNull] string name, DataType dataType)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
            TypeName = DbProviderFactories.GetFactory.GetColumnTypeName(dataType);
            DataType = dataType;
        }

        public DataType? DataType { get; }
    }
}

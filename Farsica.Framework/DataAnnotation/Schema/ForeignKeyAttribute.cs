namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class ForeignKeyAttribute : System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute
    {
        public ForeignKeyAttribute(string name)
            : base(DbProviderFactories.GetFactory.GetObjectName(name))
        {
        }
    }
}
namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class ForeignKeyAttribute(string name) : System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute(DbProviderFactories.GetFactory.GetObjectName(name))
    {
    }
}
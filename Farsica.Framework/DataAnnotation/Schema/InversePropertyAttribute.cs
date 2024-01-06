namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class InversePropertyAttribute(string property) : System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute(DbProviderFactories.GetFactory.GetObjectName(property))
    {
    }
}
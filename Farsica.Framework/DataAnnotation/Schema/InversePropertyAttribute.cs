namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class InversePropertyAttribute : System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute
    {
        public InversePropertyAttribute(string property)
            : base(DbProviderFactories.GetFactory.GetObjectName(property))
        {
        }
    }
}
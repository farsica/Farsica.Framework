namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class TableAttribute : System.ComponentModel.DataAnnotations.Schema.TableAttribute
    {
        public TableAttribute(string name, string? prefix = null, bool pluralize = true)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, prefix, pluralize))
        {
            Prefix = prefix;
            Pluralize = pluralize;
        }

        public string? Prefix { get; }

        public bool Pluralize { get; }
    }
}

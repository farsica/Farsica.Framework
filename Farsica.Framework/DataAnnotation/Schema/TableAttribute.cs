namespace Farsica.Framework.DataAnnotation.Schema
{
    using Farsica.Framework.Data;

    public sealed class TableAttribute(string name, string? prefix = null, bool pluralize = true) : System.ComponentModel.DataAnnotations.Schema.TableAttribute(DbProviderFactories.GetFactory.GetObjectName(name, prefix, pluralize))
    {
        public string? Prefix { get; } = prefix;

        public bool Pluralize { get; } = pluralize;
    }
}

namespace Farsica.Framework.Validation
{
    using System.Reflection;

    public interface IClientPropertyValidator
    {
        string? GetJsonMetaData(PropertyInfo? property);
    }
}

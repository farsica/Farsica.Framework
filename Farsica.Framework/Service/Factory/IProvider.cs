namespace Farsica.Framework.Service.Factory
{
    using Farsica.Framework.Data.Enumeration;

    public interface IProvider<T>
        where T : Enumeration<byte>
    {
        T ProviderType { get; }
    }
}

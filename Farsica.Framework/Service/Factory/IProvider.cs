namespace Farsica.Framework.Service.Factory
{
    public interface IProvider<T>
        where T : struct
    {
        T ProviderType { get; }
    }
}

namespace Farsica.Framework.Service.Factory
{
    [DataAnnotation.Injectable]
    public interface IGenericFactory<TProvider, TProviderType>
        where TProvider : IProvider<TProviderType>
        where TProviderType : struct
    {
        TProvider GetProvider(TProviderType providerType, bool returnFirstItemIfNotMatch = true);
    }
}

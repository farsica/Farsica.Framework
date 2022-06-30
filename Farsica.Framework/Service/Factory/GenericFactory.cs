namespace Farsica.Framework.Service.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GenericFactory<TProvider, TProviderType> : IGenericFactory<TProvider, TProviderType>
        where TProvider : IProvider<TProviderType>
        where TProviderType : struct
    {
        private readonly IEnumerable<TProvider> providers;

        public GenericFactory(IEnumerable<TProvider> providers)
        {
            this.providers = providers;
        }

        public TProvider GetProvider(TProviderType providerType, bool returnFirstItemIfNotMatch = true)
        {
            if (providers == null)
            {
                throw new Exception("providers is null");
            }

            var provider = providers.FirstOrDefault(t => t.ProviderType.Equals(providerType));
            if (provider == null && returnFirstItemIfNotMatch)
            {
                provider = providers.First();
            }

            return provider;
        }
    }
}

namespace Farsica.Framework.Service
{
    using System;
    using Farsica.Framework.HttpProvider;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class InfrastructureBase<T>
        where T : class
    {
        protected InfrastructureBase(Lazy<IHttpProvider> httpProvider, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        {
            HttpProvider = httpProvider;
            Localizer = localizer;
            Logger = logger;
        }

        protected Lazy<IHttpProvider> HttpProvider { get; }

        protected Lazy<IStringLocalizer<T>> Localizer { get; }

        protected Lazy<ILogger<T>> Logger { get; }
    }
}

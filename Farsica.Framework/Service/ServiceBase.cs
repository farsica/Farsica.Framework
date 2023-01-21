namespace Farsica.Framework.Service
{
    using System;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class ServiceBase<T>
        where T : class
    {
        protected ServiceBase(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Localizer = localizer;
        }

        protected Lazy<ILogger<T>> Logger { get; }

        protected Lazy<IHttpContextAccessor> HttpContextAccessor { get; }

        protected Lazy<IStringLocalizer<T>> Localizer { get; }

        protected Lazy<IUnitOfWorkProvider> UnitOfWorkProvider { get; }
    }
}

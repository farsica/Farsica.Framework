namespace Farsica.Framework.Service
{
    using System;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public abstract class ServiceBase<T>
        where T : class
    {
        protected ServiceBase(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<ILogger<T>> logger)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
        }

        protected Lazy<ILogger<T>> Logger { get; }

        protected Lazy<IHttpContextAccessor> HttpContextAccessor { get; }

        protected Lazy<IUnitOfWorkProvider> UnitOfWorkProvider { get; }
    }
}

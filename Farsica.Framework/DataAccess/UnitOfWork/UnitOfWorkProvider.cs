namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using Farsica.Framework.DataAccess.Context;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly ILogger<DataAccess> logger;
        private readonly IServiceProvider serviceProvider;

        public UnitOfWorkProvider()
        {
        }

        public UnitOfWorkProvider(ILogger<DataAccess> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false)
        {
            var context = serviceProvider.GetService(typeof(IEntityContext)) as DbContext;

            if (!trackChanges)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork(context, serviceProvider, logger);
        }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false)
            where TEntityContext : DbContext
        {
            var context = serviceProvider.GetService(typeof(IEntityContext)) as TEntityContext;

            if (!trackChanges)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork(context, serviceProvider, logger);
        }
    }
}

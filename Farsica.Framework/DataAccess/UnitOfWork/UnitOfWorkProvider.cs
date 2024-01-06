namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using Farsica.Framework.DataAccess.Context;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [ServiceLifetime(ServiceLifetime.Scoped)]
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
            var context = serviceProvider.GetRequiredService<IEntityContext>() as DbContext;

            if (!trackChanges)
            {
                context!.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork<DbContext>(context!, serviceProvider, logger);
        }

        public IUnitOfWork CreateUnitOfWork<T>(bool trackChanges = true, bool enableLogging = false)
            where T : DbContext
        {
            var context = serviceProvider.GetRequiredService<T>();

            if (!trackChanges)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork<T>(context, serviceProvider, logger);
        }
    }
}

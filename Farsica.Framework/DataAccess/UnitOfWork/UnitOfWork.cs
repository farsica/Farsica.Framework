namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class UnitOfWork<T>(T context, IServiceProvider provider, ILogger<DataAccess> logger)
        : UnitOfWorkBase<T>(context, provider, logger), IUnitOfWork
        where T : DbContext
    {
    }
}

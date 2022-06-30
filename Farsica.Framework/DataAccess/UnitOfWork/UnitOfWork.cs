namespace Farsica.Framework.DataAccess.UnitOfWork
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
    {
        public UnitOfWork(DbContext context, IServiceProvider provider, ILogger<DataAccess> logger)
            : base(context, provider, logger)
        {
        }
    }
}

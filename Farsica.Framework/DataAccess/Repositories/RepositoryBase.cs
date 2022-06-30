namespace Farsica.Framework.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public abstract class RepositoryBase<TContext> : IRepositoryInjection
        where TContext : DbContext
    {
        protected RepositoryBase(ILogger<DataAccess> logger, TContext context)
        {
            Logger = logger;
            Context = context;
        }

        protected ILogger Logger { get; }

        protected TContext Context { get; private set; }

        public IRepositoryInjection SetContext(DbContext context)
        {
            Context = (TContext)context;
            return this;
        }
    }
}

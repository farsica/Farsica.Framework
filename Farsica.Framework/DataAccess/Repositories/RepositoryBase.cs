namespace Farsica.Framework.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public abstract class RepositoryBase<TContext> : IRepositoryInjection<TContext>
        where TContext : DbContext
    {
        protected RepositoryBase(ILogger<DataAccess> logger, TContext context)
        {
            Logger = logger;
            Context = context;
        }

        protected ILogger Logger { get; }

        protected TContext Context { get; private set; }

        public IRepositoryInjection<TContext> SetContext(TContext context)
        {
            Context = context;
            return this;
        }
    }
}

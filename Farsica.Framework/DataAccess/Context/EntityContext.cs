namespace Farsica.Framework.DataAccess.Context
{
    using System;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public abstract class EntityContext<TContext> : DbContext, IEntityContext
        where TContext : DbContext
    {
        protected EntityContext(IServiceProvider serviceProvider)
            : base()
        {
            ServiceProvider = serviceProvider;

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            ConnectionName = configuration.GetValue<string>("Connection:ConnectionString") + configuration.GetValue<string>("Connection:License");
            DefaultSchema = configuration.GetValue<string>("Connection:DefaultSchema");
            SensitiveDataLoggingEnabled = configuration.GetValue<bool>("Connection:SensitiveDataLoggingEnabled");
            DetailedErrorsEnabled = configuration.GetValue<bool>("Connection:DetailedErrorsEnabled");
            LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
        }

        protected string? ConnectionName { get; }

        protected string? DefaultSchema { get; }

        protected bool SensitiveDataLoggingEnabled { get; }

        protected bool DetailedErrorsEnabled { get; }

        protected ILoggerFactory LoggerFactory { get; }

        protected abstract Assembly EntityAssembly { get; }

        private IServiceProvider ServiceProvider { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (!string.IsNullOrEmpty(DefaultSchema))
            {
                builder.HasDefaultSchema(DefaultSchema);
            }

            builder.ApplyConfigurationsFromAssembly(EntityAssembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(SensitiveDataLoggingEnabled)
                .EnableDetailedErrors(DetailedErrorsEnabled)
                .UseLoggerFactory(LoggerFactory);
        }
    }
}

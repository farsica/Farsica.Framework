namespace Farsica.Framework.DataAccess.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.DataAccess.Audit;
    using Farsica.Framework.DataAccess.Bulk;
    using Farsica.Framework.DataAccess.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class EntityContextBase<TContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IEntityContext
        where TContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        private readonly string[] shadowProperties = new[] { nameof(IVersioning<TUser, TKey>.CreationDate), nameof(IVersioning<TUser, TKey>.CreationUserId), nameof(IVersioning<TUser, TKey>.LastModifyDate), nameof(IVersioning<TUser, TKey>.LastModifyUserId) };
        private readonly IHttpContextAccessor httpContextAccessor;

        protected EntityContextBase(IServiceProvider serviceProvider)
            : base()
        {
            ServiceProvider = serviceProvider;

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            ConnectionName = configuration.GetValue<string>("Connection:ConnectionString") + configuration.GetValue<string>("Connection:License");
            DefaultSchema = configuration.GetValue<string>("Connection:DefaultSchema");
            SensitiveDataLoggingEnabled = configuration.GetValue<bool>("Connection:SensitiveDataLoggingEnabled");
            DetailedErrorsEnabled = configuration.GetValue<bool>("Connection:DetailedErrorsEnabled");
            LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public DbSet<Audit> Audits { get; set; }

        public DbSet<AuditEntry> AuditEntries { get; set; }

        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

        protected string? ConnectionName { get; }

        protected string? DefaultSchema { get; }

        protected bool SensitiveDataLoggingEnabled { get; }

        protected bool DetailedErrorsEnabled { get; }

        protected ILoggerFactory LoggerFactory { get; }

        protected abstract Assembly EntityAssembly { get; }

        protected abstract bool EnableAudit { get; }

        private IServiceProvider ServiceProvider { get; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var data = GenerateAudit();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            if (data is not null)
            {
                SaveAudit(data);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var data = GenerateAudit();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            if (data is not null)
            {
                await SaveAuditAsync(data);
            }

            return result;
        }

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

            // .ReplaceService<IMigrator, Migrator>();
        }

        private static Constants.AuditType Convert(EntityState entityState)
        {
            return entityState switch
            {
                EntityState.Deleted => Constants.AuditType.Deleted,
                EntityState.Modified => Constants.AuditType.Modified,
                EntityState.Added => Constants.AuditType.Added,
                _ => throw new ArgumentOutOfRangeException(nameof(entityState)),
            };
        }

        private Audit? GenerateAudit()
        {
            if (EnableAudit is false)
            {
                return null;
            }

            ChangeTracker.DetectChanges();
            var audit = new Audit
            {
                Date = DateTimeOffset.Now,
                IpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),
                UserId = httpContextAccessor.HttpContext?.User.UserId(),
                AuditEntries = new List<AuditEntry>(),
            };
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditAttribute = entry.Entity.GetType().GetCustomAttribute<DataAnnotation.AuditAttribute>();
                if (auditAttribute is null)
                {
                    continue;
                }

                _ = long.TryParse(entry.Properties.FirstOrDefault(t => t.Metadata.IsPrimaryKey())?.CurrentValue?.ToString(), out long id);
                var auditEntry = new AuditEntry
                {
                    EntityType = auditAttribute.EntityType,
                    AuditEntryProperties = new List<AuditEntryProperty>(),
                    AuditType = Convert(entry.State),
                    IdentifierId = id,
                };

                if (entry.State is not EntityState.Deleted)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.OriginalValue?.ToString() == property.CurrentValue?.ToString() || shadowProperties.Contains(property.Metadata.Name))
                        {
                            continue;
                        }

                        auditEntry.AuditEntryProperties.Add(new AuditEntryProperty
                        {
                            OldValue = property.OriginalValue?.ToString(),
                            NewValue = property.CurrentValue?.ToString(),
                            PropertyName = property.Metadata.Name,
                        });
                    }
                }

                audit.AuditEntries.Add(auditEntry);
            }

            return audit.AuditEntries.Any(t => t.AuditType == Constants.AuditType.Deleted || t.AuditEntryProperties?.Any() is true) ? audit : null;
        }

        private void SaveAudit(Audit audit)
        {
            if (audit == null)
            {
                return;
            }

            var auditLst = new List<Audit>
            {
                new Audit
                {
                    Date = audit.Date,
                    UserId = audit.UserId,
                    IpAddress = audit.IpAddress,
                    UserAgent = audit.UserAgent,
                },
            };
            this.BulkInsert(auditLst, new BulkConfig { SetOutputIdentity = true });

            var auditEntries = new List<AuditEntry>();
            if (audit.AuditEntries is not null)
            {
                foreach (var auditEntry in audit.AuditEntries)
                {
                    auditEntries.Add(new AuditEntry
                    {
                        AuditId = auditLst[0].Id,
                        AuditType = auditEntry.AuditType,
                        EntityType = auditEntry.EntityType,
                        IdentifierId = auditEntry.IdentifierId,
                    });
                }

                this.BulkInsert(auditEntries, new BulkConfig { SetOutputIdentity = true });

                var properties = new List<AuditEntryProperty>();
                foreach (var auditEntry in audit.AuditEntries)
                {
                    if (auditEntry.AuditEntryProperties is not null)
                    {
                        foreach (var property in auditEntry.AuditEntryProperties)
                        {
                            var item = auditEntries.Find(t => t.IdentifierId == auditEntry.IdentifierId && t.AuditType == auditEntry.AuditType && t.EntityType == auditEntry.EntityType);
                            if (item is not null)
                            {
                                property.AuditEntryId = item.Id;
                            }
                        }

                        properties.AddRange(auditEntry.AuditEntryProperties);
                    }
                }

                this.BulkInsert(properties);
            }
        }

        private async Task SaveAuditAsync(Audit audit)
        {
            if (audit is null)
            {
                return;
            }

            var auditLst = new List<Audit>
            {
                new Audit
                {
                    Date = audit.Date,
                    UserId = audit.UserId,
                    IpAddress = audit.IpAddress,
                    UserAgent = audit.UserAgent,
                },
            };
            await this.BulkInsertAsync(auditLst, new BulkConfig { SetOutputIdentity = true, });

            var auditEntries = new List<AuditEntry>();
            foreach (var auditEntry in audit.AuditEntries)
            {
                auditEntries.Add(new AuditEntry
                {
                    AuditId = auditLst[0].Id,
                    AuditType = auditEntry.AuditType,
                    EntityType = auditEntry.EntityType,
                    IdentifierId = auditEntry.IdentifierId,
                });
            }

            await this.BulkInsertAsync(auditEntries, new BulkConfig { SetOutputIdentity = true });

            var properties = new List<AuditEntryProperty>();
            foreach (var auditEntry in audit.AuditEntries)
            {
                foreach (var item in auditEntry.AuditEntryProperties)
                {
                    item.AuditEntryId = auditEntries.Find(t => t.IdentifierId == auditEntry.IdentifierId &&
                    t.AuditType == auditEntry.AuditType &&
                    t.EntityType == auditEntry.EntityType).Id;
                }

                properties.AddRange(auditEntry.AuditEntryProperties);
            }

            await this.BulkInsertAsync(properties);
        }
    }
}

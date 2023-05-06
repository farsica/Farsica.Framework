namespace Farsica.Framework.DataAccess.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.Data.Enumeration;
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
        private readonly string[] shadowProperties = new[] { nameof(IVersionableEntity<TUser, TKey, TKey>.CreationDate), nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyDate), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId) };
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

        public DbSet<Audit<TUser, TKey>> Audits { get; set; }

        public DbSet<AuditEntry<TUser, TKey>> AuditEntries { get; set; }

        public DbSet<AuditEntryProperty<TUser, TKey>> AuditEntryProperties { get; set; }

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
            PrepareShadowProperties();

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
            PrepareShadowProperties();

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

            ConfigureShadowProperties(builder);

            builder.ApplyConfigurationsFromAssembly(EntityAssembly);

            // ApplyConfigurationsFromAssembly not work for generic types, therefore must register manually
            _ = builder.Entity<AuditEntry<TUser, TKey>>().OwnEnumeration<AuditEntry<TUser, TKey>, AuditType, byte>(t => t.AuditType);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(SensitiveDataLoggingEnabled)
                .EnableDetailedErrors(DetailedErrorsEnabled)
                .UseLoggerFactory(LoggerFactory);
        }

        private static AuditType Convert(EntityState entityState)
        {
            return entityState switch
            {
                EntityState.Deleted => AuditType.Deleted,
                EntityState.Modified => AuditType.Modified,
                EntityState.Added => AuditType.Added,
                _ => throw new ArgumentOutOfRangeException(nameof(entityState)),
            };
        }

        private void ConfigureShadowProperties(ModelBuilder builder)
        {
            var types = EntityAssembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IVersionableEntity<,,>)))
                {
                    builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUser))
                            .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId)).OnDelete(DeleteBehavior.NoAction);

                    builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUser))
                        .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).OnDelete(DeleteBehavior.NoAction);

                    builder.Entity(type).Property(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).IsRequired(false);
                }
            }
        }

        private void PrepareShadowProperties()
        {
            ChangeTracker.DetectChanges();

            if (httpContextAccessor.HttpContext is null)
            {
                return;
            }

            var entries = ChangeTracker.Entries();
            var i = 0;
            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetInterface(typeof(IVersionableEntity<,,>).Name) is null)
                {
                    continue;
                }

                dynamic versionableEntity = entry.Entity;
                var userId = httpContextAccessor.HttpContext.UserId<TKey>();
                if (entry.State is EntityState.Added)
                {
                    if (userId is not null && userId.Equals(default) is false)
                    {
                        versionableEntity.CreationUserId = userId;
                    }

                    versionableEntity.CreationDate = DateTimeOffset.Now;
                }
                else if (entry.State is EntityState.Modified)
                {
                    if (userId is not null && userId.Equals(default) is false)
                    {
                        versionableEntity.LastModifyUserId = userId;
                    }

                    versionableEntity.LastModifyDate = DateTimeOffset.Now;
                }

                i++;
            }
        }

        private Audit<TUser, TKey>? GenerateAudit()
        {
            if (EnableAudit is false)
            {
                return null;
            }

            var audit = new Audit<TUser, TKey>
            {
                Date = DateTimeOffset.Now,
                IpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),
                UserId = httpContextAccessor.HttpContext.UserId<TKey>(),
                AuditEntries = new List<AuditEntry<TUser, TKey>>(),
            };
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is Audit<TUser, TKey> || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditAttribute = entry.Entity.GetType().GetCustomAttribute<DataAnnotation.AuditAttribute>();
                if (auditAttribute is null)
                {
                    continue;
                }

                _ = long.TryParse(entry.Properties.FirstOrDefault(t => t.Metadata.IsPrimaryKey())?.CurrentValue?.ToString(), out long id);
                var auditEntry = new AuditEntry<TUser, TKey>
                {
                    EntityType = auditAttribute.EntityType,
                    AuditEntryProperties = new List<AuditEntryProperty<TUser, TKey>>(),
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

                        auditEntry.AuditEntryProperties.Add(new AuditEntryProperty<TUser, TKey>
                        {
                            OldValue = property.OriginalValue?.ToString(),
                            NewValue = property.CurrentValue?.ToString(),
                            PropertyName = property.Metadata.Name,
                        });
                    }
                }

                audit.AuditEntries.Add(auditEntry);
            }

            return audit.AuditEntries.Any(t => t.AuditType?.Equals(AuditType.Deleted) is true || t.AuditEntryProperties?.Any() is true) ? audit : null;
        }

        private void SaveAudit(Audit<TUser, TKey> audit)
        {
            if (audit is null)
            {
                return;
            }

            var auditLst = new List<Audit<TUser, TKey>>
            {
                new Audit<TUser, TKey>
                {
                    Date = audit.Date,
                    UserId = audit.UserId,
                    IpAddress = audit.IpAddress,
                    UserAgent = audit.UserAgent,
                },
            };
            this.BulkInsert(auditLst, new BulkConfig { SetOutputIdentity = true });

            if (audit.AuditEntries is not null)
            {
                var auditEntries = new List<AuditEntry<TUser, TKey>>();
                foreach (var auditEntry in audit.AuditEntries)
                {
                    auditEntries.Add(new AuditEntry<TUser, TKey>
                    {
                        AuditId = auditLst[0].Id,
                        AuditType = auditEntry.AuditType,
                        EntityType = auditEntry.EntityType,
                        IdentifierId = auditEntry.IdentifierId,
                    });
                }

                this.BulkInsert(auditEntries, new BulkConfig { SetOutputIdentity = true });

                var properties = new List<AuditEntryProperty<TUser, TKey>>();
                foreach (var auditEntry in audit.AuditEntries)
                {
                    if (auditEntry.AuditEntryProperties is not null)
                    {
                        foreach (var property in auditEntry.AuditEntryProperties)
                        {
                            var item = auditEntries.Find(t => t.IdentifierId == auditEntry.IdentifierId && t.AuditType?.Equals(auditEntry.AuditType) is true && t.EntityType == auditEntry.EntityType);
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

        private async Task SaveAuditAsync(Audit<TUser, TKey> audit)
        {
            if (audit is null)
            {
                return;
            }

            var auditLst = new List<Audit<TUser, TKey>>
            {
                new Audit<TUser, TKey>
                {
                    Date = audit.Date,
                    UserId = audit.UserId,
                    IpAddress = audit.IpAddress,
                    UserAgent = audit.UserAgent,
                },
            };
            await this.BulkInsertAsync(auditLst, new BulkConfig { SetOutputIdentity = true, });

            if (audit.AuditEntries?.Count > 0)
            {
                var auditEntries = new List<AuditEntry<TUser, TKey>>();
                foreach (var auditEntry in audit.AuditEntries)
                {
                    auditEntries.Add(new AuditEntry<TUser, TKey>
                    {
                        AuditId = auditLst[0].Id,
                        AuditType = auditEntry.AuditType,
                        EntityType = auditEntry.EntityType,
                        IdentifierId = auditEntry.IdentifierId,
                    });
                }

                await this.BulkInsertAsync(auditEntries, new BulkConfig { SetOutputIdentity = true });

                var properties = new List<AuditEntryProperty<TUser, TKey>>();
                foreach (var auditEntry in audit.AuditEntries)
                {
                    if (auditEntry.AuditEntryProperties?.Count > 0)
                    {
                        foreach (var property in auditEntry.AuditEntryProperties)
                        {
                            var item = auditEntries.Find(t => t.IdentifierId == auditEntry.IdentifierId && t.AuditType?.Equals(auditEntry.AuditType) is true && t.EntityType == auditEntry.EntityType);
                            if (item is not null)
                            {
                                property.AuditEntryId = item.Id;
                            }
                        }

                        properties.AddRange(auditEntry.AuditEntryProperties);
                    }
                }

                await this.BulkInsertAsync(properties);
            }
        }
    }
}

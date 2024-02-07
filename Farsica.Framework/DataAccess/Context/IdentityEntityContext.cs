namespace Farsica.Framework.DataAccess.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAccess.Audit;
    using Farsica.Framework.DataAccess.Bulk;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAccess.ValueConversion;
    using Farsica.Framework.DataAccess.ValueGeneration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NUlid;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class IdentityEntityContext<TContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
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
        private readonly string[] shadowProperties = [nameof(IVersionableEntity<TUser, TKey, TKey>.CreationDate), nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyDate), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)];
        private readonly IHttpContextAccessor httpContextAccessor;

        protected IdentityEntityContext(IServiceProvider serviceProvider)
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
            SaveAudit(data);

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PrepareShadowProperties();

            var data = GenerateAudit();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await SaveAuditAsync(data);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (!string.IsNullOrEmpty(DefaultSchema))
            {
                _ = builder.HasDefaultSchema(DefaultSchema);
            }

            ConfigureProperties(EntityAssembly.GetTypes());
            ConfigureProperties(typeof(DataAccess).Assembly.GetTypes());

            builder.ApplyConfigurationsFromAssembly(EntityAssembly);

            // ApplyConfigurationsFromAssembly not work for generic types, therefore must register manually
            _ = builder.Entity<AuditEntry<TUser, TKey>>().OwnEnumeration<AuditEntry<TUser, TKey>, AuditType, byte>(t => t.AuditType);

            _ = builder.Entity<Audit<TUser, TKey>>().Property(t => t.Id).HasValueGenerator<UlidGenerator>();
            _ = builder.Entity<AuditEntry<TUser, TKey>>().Property(t => t.Id).HasValueGenerator<UlidGenerator>();
            _ = builder.Entity<AuditEntryProperty<TUser, TKey>>().Property(t => t.Id).HasValueGenerator<UlidGenerator>();

            void ConfigureProperties(Type[] types)
            {
                for (int i = 0; i < types.Length; i++)
                {
                    Type? type = types[i];
                    Type[] interfaces = type.GetInterfaces();
                    if (interfaces.Exists(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntity<,>)))
                    {
                        if (interfaces.Exists(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IVersionableEntity<,,>)))
                        {
                            builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUser))
                                    .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId)).OnDelete(DeleteBehavior.NoAction);

                            builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUser))
                                .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).OnDelete(DeleteBehavior.NoAction);

                            builder.Entity(type).Property(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).IsRequired(false);
                        }

                        if (type.IsGenericType)
                        {
                            continue;
                        }

                        PropertyInfo[] properties = type.GetProperties();
                        for (int j = 0; j < properties.Length; j++)
                        {
                            PropertyInfo? property = properties[j];

                            if (property.PropertyType == typeof(Guid) && property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                            {
                                builder.Entity(type).Property(property.PropertyType, property.Name).HasDefaultValueSql("NEWSEQUENTIALID()");
                            }
                            else if (property.PropertyType == typeof(Ulid) && property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                            {
                                builder.Entity(type).Property(property.Name).ValueGeneratedOnAdd().HasValueGenerator<UlidGenerator>();
                            }
                        }
                    }
                }
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<Ulid>().HaveConversion<UlidValueConverter>();
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

        private static void PrepareAuditIdentifierIds(Audit<TUser, TKey>? audit)
        {
            if (audit?.AuditEntries is null)
            {
                return;
            }

            for (int i = 0; i < audit.AuditEntries.Count; i++)
            {
                AuditEntry<TUser, TKey>? entry = audit.AuditEntries[i];
                if (entry.AuditEntryProperties is null)
                {
                    continue;
                }

                for (int j = 0; j < entry.AuditEntryProperties.Count; j++)
                {
                    var property = entry.AuditEntryProperties[j];
                    if (property.TemporaryProperty is not null)
                    {
                        property.NewValue = property.TemporaryProperty.CurrentValue?.ToString();
                        if (property.TemporaryProperty.Metadata.IsPrimaryKey())
                        {
                            entry.IdentifierId = property.TemporaryProperty.CurrentValue?.ToString();
                        }
                    }
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
                Id = Ulid.NewUlid(),
                Date = DateTimeOffset.Now,
                IpAddress = httpContextAccessor.HttpContext.GetClientIpAddress(),
                UserAgent = httpContextAccessor.HttpContext.UserAgent(),
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

                var auditAttribute = entry.Entity.GetType().GetCustomAttribute<AuditAttribute>();
                if (auditAttribute is null)
                {
                    continue;
                }

                var auditEntry = new AuditEntry<TUser, TKey>
                {
                    Id = Ulid.NewUlid(),
                    AuditId = audit.Id,
                    EntityType = auditAttribute.EntityType,
                    AuditEntryProperties = new List<AuditEntryProperty<TUser, TKey>>(),
                    AuditType = Convert(entry.State),
                };

                if (entry.State == EntityState.Added)
                {
                    var ids = entry.Properties.Where(t => t.Metadata.IsPrimaryKey() && !t.IsTemporary).Select(t => t.CurrentValue?.ToString());
                    auditEntry.IdentifierId = ids?.Any() == true ? string.Join(" , ", ids) : null;
                }
                else
                {
                    auditEntry.IdentifierId = entry.State == EntityState.Added ? null : string.Join(" , ", entry.Properties.Where(t => t.Metadata.IsPrimaryKey()).Select(t => t.CurrentValue?.ToString()));
                }

                if (entry.State is not EntityState.Deleted)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (entry.Entity.GetType().GetProperty(property.Metadata.Name)?.GetCustomAttribute<AuditIgnoreAttribute>() is not null)
                        {
                            continue;
                        }

                        if (shadowProperties.Contains(property.Metadata.Name))
                        {
                            continue;
                        }

                        if (entry.State == EntityState.Added || property.OriginalValue?.ToString() != property.CurrentValue?.ToString())
                        {
                            auditEntry.AuditEntryProperties.Add(new AuditEntryProperty<TUser, TKey>
                            {
                                OldValue = entry.State == EntityState.Added ? null : property.OriginalValue?.ToString(),
                                NewValue = property.CurrentValue?.ToString(),
                                PropertyName = property.Metadata.Name,
                                TemporaryProperty = property.IsTemporary ? property : null,
                                Id = Ulid.NewUlid(),
                                AuditEntryId = auditEntry.Id,
                            });
                        }
                    }
                }

                if (entry.State is EntityState.Deleted || auditEntry.AuditEntryProperties.Count > 0)
                {
                    audit.AuditEntries.Add(auditEntry);
                }
            }

            return audit.AuditEntries.Any(t => t.AuditType?.Equals(AuditType.Deleted) == true || t.AuditEntryProperties?.Count > 0) ? audit : null;
        }

        private void SaveAudit(Audit<TUser, TKey>? audit)
        {
            PrepareAuditIdentifierIds(audit);
            if (audit is not null)
            {
                this.BulkInsert([audit!], new BulkConfig { SetOutputIdentity = true, IncludeGraph = true });
            }
        }

        private async Task SaveAuditAsync(Audit<TUser, TKey>? audit)
        {
            PrepareAuditIdentifierIds(audit);
            if (audit is not null)
            {
                await this.BulkInsertAsync([audit!], new BulkConfig { SetOutputIdentity = true, IncludeGraph = true });
            }
        }
    }
}

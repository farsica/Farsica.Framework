namespace Farsica.Framework.DataAccess.Audit
{
    using System;
    using System.Collections.Generic;
    using Farsica.Framework.Data;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NUlid;

    [Table(nameof(AuditEntry<TUser, TKey>))]
    public class AuditEntry<TUser, TKey> : IEntity<AuditEntry<TUser, TKey>, Ulid>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        public Ulid Id { get; set; }

        [Column(nameof(AuditId), DataType.Ulid)]
        public Ulid AuditId { get; set; }

        public Audit<TUser, TKey>? Audit { get; set; }

        [Column(nameof(AuditType), DataType.Byte)]
        [Required]
        public AuditType? AuditType { get; set; }

        [Column(nameof(EntityType), DataType.Int)]
        public int EntityType { get; set; }

        [Column(nameof(IdentifierId), DataType.String)]
        [StringLength(100)]
        public string? IdentifierId { get; set; }

        public IList<AuditEntryProperty<TUser, TKey>>? AuditEntryProperties { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntry<TUser, TKey>> builder)
        {
            // not working, go to IdentityEntityContext
            _ = builder.OwnEnumeration<AuditEntry<TUser, TKey>, AuditType, byte>(t => t.AuditType);
        }
    }
}

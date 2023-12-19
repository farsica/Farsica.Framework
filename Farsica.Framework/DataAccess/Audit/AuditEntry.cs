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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(AuditEntry<TUser, TKey>))]
    public class AuditEntry<TUser, TKey> : IEntity<AuditEntry<TUser, TKey>, Guid>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Guid)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(nameof(AuditId), DataType.Long)]
        public Guid AuditId { get; set; }

        public Audit<TUser, TKey>? Audit { get; set; }

        [Column(nameof(AuditType), DataType.Byte)]
        [Required]
        public AuditType? AuditType { get; set; }

        [Column(nameof(EntityType), DataType.Int)]
        public int EntityType { get; set; }

        [Column(nameof(IdentifierId), DataType.String)]
        [StringLength(100)]
        public string? IdentifierId { get; set; }

        public ICollection<AuditEntryProperty<TUser, TKey>>? AuditEntryProperties { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntry<TUser, TKey>> builder)
        {
            _ = builder.Property(t => t.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            _ = builder.OwnEnumeration<AuditEntry<TUser, TKey>, AuditType, byte>(t => t.AuditType);
        }
    }
}

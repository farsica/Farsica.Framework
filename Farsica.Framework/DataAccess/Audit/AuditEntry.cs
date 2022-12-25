namespace Farsica.Framework.DataAccess.Audit
{
    using System;
    using System.Collections.Generic;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(AuditEntry<TUser, TKey>))]
    public class AuditEntry<TUser, TKey> : IEntity<AuditEntry<TUser, TKey>, long>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(nameof(AuditId), DataType.Long)]
        public long AuditId { get; set; }

        public Audit<TUser, TKey>? Audit { get; set; }

        [Column(nameof(AuditType), DataType.Byte)]
        public Core.Constants.AuditType AuditType { get; set; }

        [Column(nameof(EntityType), DataType.Int)]
        public int EntityType { get; set; }

        [Column(nameof(IdentifierId), DataType.Long)]
        public long IdentifierId { get; set; }

        public ICollection<AuditEntryProperty<TUser, TKey>>? AuditEntryProperties { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntry<TUser, TKey>> builder)
        {
        }
    }
}

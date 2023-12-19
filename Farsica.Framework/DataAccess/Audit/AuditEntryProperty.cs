namespace Farsica.Framework.DataAccess.Audit
{
    using System;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(AuditEntryProperty<TUser, TKey>))]
    public class AuditEntryProperty<TUser, TKey> : IEntity<AuditEntryProperty<TUser, TKey>, Guid>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Guid)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(nameof(AuditEntryId), DataType.Guid)]
        public Guid AuditEntryId { get; set; }

        public AuditEntry<TUser, TKey>? AuditEntry { get; set; }

        [StringLength(50)]
        [Required]
        [Column(nameof(PropertyName), DataType.String)]
        public string? PropertyName { get; set; }

        [Column(nameof(OldValue), DataType.UnicodeMaxString)]
        public string? OldValue { get; set; }

        [Column(nameof(NewValue), DataType.UnicodeMaxString)]
        public string? NewValue { get; set; }

        [NotMapped]
        public PropertyEntry? TemporaryProperty { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntryProperty<TUser, TKey>> builder)
        {
            // not working, go to IdentityEntityContext
            _ = builder.Property(t => t.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}

namespace Farsica.Framework.DataAccess.Audit
{
    using System.Collections.Generic;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(AuditEntry))]
    public class AuditEntry : IEntity<AuditEntry, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(nameof(AuditId), DataType.Long)]
        public long AuditId { get; set; }

        public Audit? Audit { get; set; }

        [Column(nameof(AuditType), DataType.Byte)]
        public Core.Constants.AuditType AuditType { get; set; }

        [Column(nameof(EntityType), DataType.Int)]
        public int EntityType { get; set; }

        [Column(nameof(IdentifierId), DataType.Long)]
        public long IdentifierId { get; set; }

        public ICollection<AuditEntryProperty>? AuditEntryProperties { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
        }
    }
}

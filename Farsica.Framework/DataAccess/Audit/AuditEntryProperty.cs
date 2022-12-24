namespace Farsica.Framework.DataAccess.Audit
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(AuditEntryProperty))]
    public class AuditEntryProperty : IEntity<AuditEntryProperty, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(nameof(AuditEntryId), DataType.Long)]
        public long AuditEntryId { get; set; }

        public AuditEntry? AuditEntry { get; set; }

        [StringLength(50)]
        [Required]
        [Column(nameof(PropertyName), DataType.String)]
        public string? PropertyName { get; set; }

        [Column(nameof(OldValue), DataType.UnicodeMaxString)]
        public string? OldValue { get; set; }

        [Column(nameof(NewValue), DataType.UnicodeMaxString)]
        public string? NewValue { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntryProperty> builder)
        {
        }
    }
}

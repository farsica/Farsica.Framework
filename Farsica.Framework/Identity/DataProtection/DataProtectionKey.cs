namespace Farsica.Framework.Identity.DataProtection
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NUlid;

    [Table(nameof(DataProtectionKey))]
    public class DataProtectionKey : IEntity<DataProtectionKey, Ulid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid Id { get; set; }

        [Column(nameof(FriendlyName), DataType.UnicodeString)]
        [StringLength(50)]
        public string? FriendlyName { get; set; }

        [Column(nameof(Xml), DataType.UnicodeMaxString)]
        public string? Xml { get; set; }

        public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
        {
        }
    }
}

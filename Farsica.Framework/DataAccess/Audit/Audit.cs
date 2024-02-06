namespace Farsica.Framework.DataAccess.Audit
{
    using System;
    using System.Collections.Generic;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NUlid;

    [Table(nameof(Audit<TUser, TKey>))]
    public class Audit<TUser, TKey> : IEntity<Audit<TUser, TKey>, Ulid>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid Id { get; set; }

        [Column(nameof(UserId))]
        public TKey? UserId { get; set; }

        public TUser? User { get; set; }

        [Column(nameof(Date), DataType.DateTimeOffset)]
        public DateTimeOffset Date { get; set; }

        [StringLength(1000)]
        [Column(nameof(UserAgent), DataType.UnicodeString)]
        [Required]
        public string? UserAgent { get; set; }

        [StringLength(50)]
        [Column(nameof(IpAddress), DataType.String)]
        [Required]
        public string? IpAddress { get; set; }

        public IList<AuditEntry<TUser, TKey>>? AuditEntries { get; set; }

        public void Configure(EntityTypeBuilder<Audit<TUser, TKey>> builder)
        {
            // not working, go to IdentityEntityContext
        }
    }
}

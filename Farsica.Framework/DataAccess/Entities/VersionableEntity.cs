namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class VersionableEntity<TUser, TCreationKey, TLastModifyKey> : IVersionableEntity<TUser, TCreationKey, TLastModifyKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        where TUser : IdentityUser<TCreationKey>
        where TCreationKey : IEquatable<TCreationKey>
    {
        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        [Required]
        public virtual DateTimeOffset CreationDate { get; set; }

        [Column(nameof(CreationUserId))]
        [Required]
        public virtual TCreationKey CreationUserId { get; set; }

        public TUser CreationUser { get; set; }

        [Column(nameof(LastModifyDate), Data.DataType.DateTimeOffset)]
        public virtual DateTimeOffset? LastModifyDate { get; set; }

        [Column(nameof(LastModifyUserId))]
        public virtual TLastModifyKey? LastModifyUserId { get; set; }

        public TUser? LastModifyUser { get; set; }
    }
}

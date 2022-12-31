namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;

    public abstract class VersionableEntity<TUser, TUserKey> : IVersionableEntity<TUser, TUserKey>
        where TUser : IdentityUser<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        public DateTimeOffset CreationDate { get; set; }

        [Column(nameof(CreationUserId))]
        public TUserKey CreationUserId { get; set; }

        public TUser CreationUser { get; set; }

        [Column(nameof(LastModifyDate), Data.DataType.DateTimeOffset)]
        public DateTimeOffset? LastModifyDate { get; set; }

        [Column(nameof(LastModifyUserId))]
        public TUserKey? LastModifyUserId { get; set; }

        public TUser? LastModifyUser { get; set; }
    }
}

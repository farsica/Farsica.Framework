namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Farsica.Framework.DataAnnotation.Schema;
    using Microsoft.AspNetCore.Identity;

    public interface IVersionableEntity<TUser, TUserKey>
        where TUser : IdentityUser<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        DateTimeOffset CreationDate { get; set; }

        [Column(nameof(CreationDate))]
        TUserKey CreationUserId { get; set; }

        TUser CreationUser { get; set; }

        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        DateTimeOffset? LastModifyDate { get; set; }

        [Column(nameof(CreationDate))]
        TUserKey? LastModifyUserId { get; set; }

        TUser? LastModifyUser { get; set; }
    }
}

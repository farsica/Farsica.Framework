namespace Farsica.Framework.Identity
{
    using System;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Identity;

    [Injectable]
    public interface IApiDataProtectorTokenProvider<TUser, TKey> : IUserTwoFactorTokenProvider<TUser>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}

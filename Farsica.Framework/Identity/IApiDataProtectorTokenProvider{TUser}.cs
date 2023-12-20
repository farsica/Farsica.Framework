namespace Farsica.Framework.Identity
{
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Identity;

    [Injectable]
    public interface IApiDataProtectorTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : class
    {
    }
}

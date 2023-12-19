namespace Farsica.Framework.Identity
{
    using System;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ApiDataProtectorTokenProvider<TUser, TKey>(IDataProtectionProvider dataProtectionProvider, IOptions<ApiDataProtectorTokenProviderOptions> options, ILogger<ApiDataProtectorTokenProvider<TUser, TKey>> logger)
        : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger), IApiDataProtectorTokenProvider<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}

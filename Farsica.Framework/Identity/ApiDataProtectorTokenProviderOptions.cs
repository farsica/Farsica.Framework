namespace Farsica.Framework.Identity
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    public class ApiDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public ApiDataProtectorTokenProviderOptions()
        {
            Name = PermissionConstants.ApiDataProtectorTokenProvider;
        }

        public static TimeSpan GetTokenLifespan(IConfiguration configuration)
        {
            return configuration.GetValue<TimeSpan>("IdentityOptions:Tokens:ApiDataProtectorTokenProviderOptions:TokenLifespan");
        }
    }
}

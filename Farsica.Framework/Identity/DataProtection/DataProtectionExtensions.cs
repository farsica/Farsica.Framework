namespace Farsica.Framework.Identity.DataProtection
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    internal static class DataProtectionExtensions
    {
        public static IDataProtectionBuilder PersistKeysToDbContext(this IDataProtectionBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            {
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new CustomXmlRepository(services);
                });
            });

            return builder;
        }
    }
}

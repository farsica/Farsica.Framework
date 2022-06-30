namespace Farsica.Framework.Startup
{
    using System.Collections.Generic;
    using System.Linq;

    using Farsica.Framework.Core;
    using Farsica.Framework.Localization;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class LocalizationExtensions
    {
        public static RequestLocalizationOptions RequestLocalizationOptions
        {
            get
            {
                var supportedCultures = CultureExtensions.GetAtomicValues().Select(t => Globals.GetCulture(t)).ToArray();
                return new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures,
                    RequestCultureProviders = new List<IRequestCultureProvider> { new RouteValueRequestCultureProvider(supportedCultures) },
                };
            }
        }

        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var supportedCultures = CultureExtensions.GetAtomicValues().Select(t => Globals.GetCulture(t)).ToArray();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(supportedCultures));
            });
        }
    }
}

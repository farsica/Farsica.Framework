namespace Farsica.Framework.Localization
{
    using System.Linq;

    using Farsica.Framework.Core;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class LocalizationExtensions
    {
        public static RequestLocalizationOptions RequestLocalizationOptions
        {
            get
            {
                var supportedCultures = CultureExtensions.GetAtomicValues().Select(Globals.GetCulture).ToArray();
                return new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures,
                    RequestCultureProviders = [new RouteValueRequestCultureProvider(supportedCultures)],
                };
            }
        }

        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var supportedCultures = CultureExtensions.GetAtomicValues().Select(Globals.GetCulture).ToArray();

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

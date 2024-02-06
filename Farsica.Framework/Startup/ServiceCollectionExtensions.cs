namespace Farsica.Framework.Startup
{
    using System;
    using Farsica.Framework.Cookie;
    using Farsica.Framework.Swagger;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services, string? loginAction, string? loginController, string? loginArea = null)
        {
            return services.AddTransient((IServiceProvider serviceProvider) =>
            {
                return new CookieAuthenticationEvents(serviceProvider.GetRequiredService<IUrlHelperFactory>(), loginAction, loginController, loginArea);
            }).ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(CookieAuthenticationEvents);
            });
        }

        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services, string? loginPageName, string? loginArea = null)
        {
            return services.AddTransient((IServiceProvider serviceProvider) =>
            {
                return new CookieAuthenticationEvents(serviceProvider.GetRequiredService<IUrlHelperFactory>(), page: loginPageName, area: loginArea);
            }).ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(CookieAuthenticationEvents);
            });
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, Action<SwaggerGenOptions>? setupAction = null)
        {
            Action<SwaggerGenOptions> baseSetupAction = options =>
            {
                options.SchemaFilter<EnumerationToEnumSchemaFilter>();
                options.SchemaFilter<UlidToStringSchemaFilter>();

                options.DocumentFilter<DisplayNameDocumentFilter>();

                options.OperationFilter<DisplayNameOperationFilter>();
            };

            return services.AddEndpointsApiExplorer().AddSwaggerGen(baseSetupAction + setupAction);
        }
    }
}

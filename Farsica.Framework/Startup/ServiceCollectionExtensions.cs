namespace Farsica.Framework.Startup
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Farsica.Framework.Cookie;
    using Farsica.Framework.Core;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Swagger;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
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

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string? defaultNamespace, Action<SwaggerGenOptions>? setupAction = null)
        {
            Action<SwaggerGenOptions> baseSetupAction = options =>
            {
                options.SchemaFilter<EnumerationToEnumSchemaFilter>();
                options.SchemaFilter<UlidToStringSchemaFilter>();

                options.DocumentFilter<DisplayNameDocumentFilter>();

                options.OperationFilter<DisplayNameOperationFilter>();

                var frameworkAssembly = Assembly.GetExecutingAssembly();
                var dir = Path.GetDirectoryName(frameworkAssembly.Location);
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }

                var files = Directory.GetFiles(dir, $"{defaultNamespace}.*.dll").Where(t => !t.Contains(frameworkAssembly.ManifestModule.Name!, StringComparison.OrdinalIgnoreCase));
                var assemblies = files.Select(Assembly.LoadFrom)
                    .Where(t => t.GetCustomAttribute<InjectableAttribute>() is not null)
                    .Union(new[] { frameworkAssembly });
                var allTypes = assemblies.SelectMany(t => t.DefinedTypes);
                var customConstraintMaps = typeof(IRouteConstraint).GetAllTypesImplementingType(allTypes);
                if (customConstraintMaps is not null)
                {
                    foreach (var item in customConstraintMaps)
                    {
                        options.ParameterFilterDescriptors.Add(new FilterDescriptor
                        {
                            Type = typeof(EnumerationParameterFilter),
                            Arguments = [item],
                        });
                    }
                }
            };

            return services.AddEndpointsApiExplorer().AddSwaggerGen(baseSetupAction + setupAction);
        }
    }
}

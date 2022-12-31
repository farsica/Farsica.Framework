namespace Farsica.Framework.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Context;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Identity;
    using Farsica.Framework.Localization;
    using Farsica.Framework.Mapping;
    using Farsica.Framework.Mvc.Routing;
    using Farsica.Framework.Mvc.ViewFeatures;
    using Farsica.Framework.Resources;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.WebEncoders;

    public abstract class Startup<TUser, TRole>
        where TUser : class
        where TRole : class
    {
        private readonly ExceptionHandlerOptions? exceptionHandlerOptions;
        private readonly bool localization;
        private readonly bool authentication;
        private readonly bool razorPages;
        private readonly bool antiforgery;
        private readonly bool https;
        private readonly bool views;
        private readonly bool identity;
        private readonly string? defaultNamespace;

        protected Startup(IConfiguration configuration, string? defaultNamespace = null, ExceptionHandlerOptions? exceptionHandlerOptions = null, bool localization = true, bool authentication = true,
            bool razorPages = true, bool antiforgery = true, bool https = true, bool views = true, bool identity = true)
        {
            Configuration = configuration;
            this.exceptionHandlerOptions = exceptionHandlerOptions;
            this.localization = localization;
            this.authentication = authentication;
            this.razorPages = razorPages;
            this.antiforgery = antiforgery;
            this.https = https;
            this.views = views;
            this.identity = identity;
            this.defaultNamespace = defaultNamespace ?? "Farsica";
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Assembly frameworkAssembly = Assembly.GetExecutingAssembly();
            var dir = Path.GetDirectoryName(frameworkAssembly.Location);
            if (string.IsNullOrEmpty(dir))
            {
                return;
            }

            var files = Directory.GetFiles(dir, $"{defaultNamespace}.*.dll").Where(t => !t.Contains(frameworkAssembly.ManifestModule.Name));

            var uiAssembly = files.FirstOrDefault(t => t.EndsWith(".UI.Web.dll"));
            if (string.IsNullOrEmpty(uiAssembly))
            {
                return;
            }

            var applicationName = Path.GetFileNameWithoutExtension(uiAssembly);
            var mvcBuilder = ConfigureServicesInternal(services, dir, applicationName);

            AddScopedDynamic(services, frameworkAssembly, files);
            ConfigureServicesCore(services, mvcBuilder);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (localization)
            {
                // app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
                app.UseRequestLocalization(LocalizationExtensions.RequestLocalizationOptions);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                if (exceptionHandlerOptions is not null)
                {
                    app.UseExceptionHandler(exceptionHandlerOptions);
                }
                else
                {
                    app.UseExceptionHandler("/error");
                }

                if (https)
                {
                    app.UseHsts();
                    app.UseHttpsRedirection();
                }
            }

            app.UseRouting();

            if (authentication)
            {
                app.UseAuthentication().UseAuthorization();
            }

            // env.WebRootFileProvider = new CompositeFileProvider(env.WebRootFileProvider, new ManifestEmbeddedFileProvider(Assembly.GetCallingAssembly(), "wwwroot"));
            if (views || razorPages)
            {
                app.UseStaticFiles();
            }

            ConfigureCore(app, env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{culture=fa}/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=fa}/{controller:slugify=Home}/{action:slugify=Index}/{id?}");

                if (razorPages)
                {
                    endpoints.MapRazorPages();
                }

                // var routes = app.ApplicationServices.GetRequiredService<Dashboard.RouteCollection>();
                // const string pattern = "/powershell";
                // var pipeline = app
                // .UsePathBase(pattern)
                // .UseMiddleware<PowershellMiddleware>(routes)
                // .Build();
                // endpoints.Map(pattern+"/{**path}", pipeline);
            });

            // var routes = app.ApplicationServices.GetRequiredService<RouteCollection>();
            // app.Map(new PathString("/powershell"), t => t.UseMiddleware<PowershellMiddleware>(routes));
        }

        protected abstract void ConfigureServicesCore(IServiceCollection services, IMvcBuilder mvcBuilder);

        protected abstract void ConfigureCore(IApplicationBuilder app, IWebHostEnvironment env);

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType is null)
            {
                throw new InvalidOperationException("NotIdentityUser");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            if (roleType is not null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
                if (identityRoleType is null)
                {
                    throw new InvalidOperationException("NotIdentityRole");
                }

                Type userStoreType;
                Type roleStoreType;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,,,>));
                if (identityContext is null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(UserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                    roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(UserStore<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[5],
                        identityContext.GenericTypeArguments[7],
                        identityContext.GenericTypeArguments[6]);
                    roleStoreType = typeof(RoleStore<,,,,>).MakeGenericType(roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[6]);
                }

                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
                services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            }
            else
            { // No Roles
                Type userStoreType;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityUserContext<,,,,>));
                if (identityContext is null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(UserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(UserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                        identityContext.GenericTypeArguments[1],
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4]);
                }

                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            }
        }

        private static TypeInfo? FindGenericBaseType([NotNull] Type currentType, [NotNull] Type genericBaseType)
        {
            var type = currentType;
            while (type is not null)
            {
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType is not null && genericType == genericBaseType)
                {
                    return type.GetTypeInfo();
                }

                type = type.BaseType;
            }

            return null;
        }

        private IMvcBuilder ConfigureServicesInternal(IServiceCollection services, string dir, string applicationName)
        {
            services.AddTransient(typeof(Lazy<>));

            if (localization)
            {
                services.TryAddSingleton<Microsoft.Extensions.Localization.IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

                if (views || razorPages)
                {
                    services.TryAddSingleton<IHtmlGenerator, HtmlGenerator>();
                }

                services.AddLocalization();
                services.ConfigureRequestLocalization();
            }

            services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                options.LowercaseUrls = true;
            });

            Action<MvcOptions> configureMvc = options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBinding.DisplayMetadataProvider());

                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((v) => string.Format(GlobalResource.Validation_MissingBindRequiredValueAccessor, v));
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => GlobalResource.Validation_MissingKeyOrValueAccessor);
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => GlobalResource.Validation_MissingRequestBodyRequiredValueAccessor);
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((v) => string.Format(GlobalResource.Validation_ValueMustNotBeNullAccessor, v));
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((v1, v2) => string.Format(GlobalResource.Validation_AttemptedValueIsInvalidAccessor, v1, v2));
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((v) => string.Format(GlobalResource.Validation_NonPropertyAttemptedValueIsInvalidAccessor, v));
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((v) => string.Format(GlobalResource.Validation_UnknownValueIsInvalidAccessor, v));
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => GlobalResource.Validation_NonPropertyUnknownValueIsInvalidAccessor);
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((v) => string.Format(GlobalResource.Validation_ValueIsInvalidAccessor, v));
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((v) => string.Format(GlobalResource.Validation_ValueMustBeANumberAccessor, v));
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => string.Format(GlobalResource.Validation_NonPropertyValueMustBeANumberAccessor));
            };
            var mvcBuilder = views ? services.AddControllersWithViews(configureMvc) : services.AddControllers(configureMvc);

            if (localization)
            {
                if (razorPages || views)
                {
                    mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
                }
            }

            mvcBuilder.AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(GlobalResource));
            });

            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new ApiResponse<object>(actionContext.ModelState));

                    // var error = new Dictionary<string, string>();
                    // foreach (var key in actionContext.ModelState.Keys)
                    // {
                    //    foreach (var parameter in actionContext.ActionDescriptor.Parameters)
                    //    {
                    //        var prop = parameter.ParameterType.GetProperty(key);
                    //        if (prop != null)
                    //        {
                    //            var attr = prop.GetCustomAttributes(typeof(ValidationAttribute), false).FirstOrDefault() as ValidationAttribute;
                    //            if (attr is MinLengthAttribute minLengthAttribute)
                    //            {
                    //                error.Add("Error", "minLength");
                    //                error.Add("Property", key);
                    //                error.Add("minimum", minLengthAttribute.Length.ToString());
                    //            }
                    //        }
                    //    }
                    // }

                    // return new BadRequestObjectResult(error);
                };
            });

            if (razorPages)
            {
                services.AddRazorPages(options =>
                {
                    options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
                    options.Conventions.Add(new CultureTemplateRouteModelConvention());
                });
            }

            if (antiforgery)
            {
                services.AddAntiforgery(options =>
                {
                    options.Cookie.Name = Constants.RequestVerificationTokenCookie;
                });
                var keysFolder = Path.Combine(dir, "frb-keys");
                services.AddDataProtection()
                    .SetApplicationName(applicationName)
                    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(12));
            }

            var embeddedFileProvider = new EmbeddedFileProvider(Assembly.GetCallingAssembly(), "Farsica.Framework");
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(embeddedFileProvider);
            });

            services.AddHttpClient(Constants.HttpClientIgnoreSslAndAutoRedirect).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                },
                AllowAutoRedirect = false,
            });

            services.AddHttpContextAccessor();
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            return mvcBuilder;
        }

        private void AddScopedDynamic(IServiceCollection services, Assembly frameworkAssembly, IEnumerable<string> assemblyFiles)
        {
            var assemblies = assemblyFiles.Select(t => Assembly.LoadFrom(t))
                .Where(t => t.GetCustomAttribute<InjectableAttribute>() is not null)
                .Union(new[] { frameworkAssembly });
            var allTypes = assemblies.SelectMany(t => t.DefinedTypes);
            var injectableTypes = allTypes.Where(t => t.GetCustomAttribute<InjectableAttribute>() is not null && (razorPages || views || t.Namespace.StartsWith("Farsica.Framework.UI") is false));
            foreach (var serviceType in injectableTypes)
            {
                var implementationTypes = serviceType.GetAllTypesImplementingType(allTypes);
                if (implementationTypes is null || implementationTypes.Any() is false)
                {
                    continue;
                }

                foreach (var implementationType in implementationTypes)
                {
                    var serviceLifetimeAttribute = implementationType.GetCustomAttribute<ServiceLifetimeAttribute>();
                    serviceLifetimeAttribute ??= new ServiceLifetimeAttribute(ServiceLifetime.Transient);

                    if (serviceLifetimeAttribute.Parameters?.Any() == true)
                    {
                        services.Add(new ServiceDescriptor(serviceType, provider =>
                        {
                            var parameters = serviceLifetimeAttribute.Parameters.Select(t => provider.GetService(t)).ToArray();
                            return Activator.CreateInstance(implementationType, parameters);
                        }, serviceLifetimeAttribute.ServiceLifetime));

                        services.Add(new ServiceDescriptor(implementationType, provider =>
                        {
                            var parameters = serviceLifetimeAttribute.Parameters.Select(t => provider.GetService(t)).ToArray();
                            return Activator.CreateInstance(implementationType, parameters);
                        }, serviceLifetimeAttribute.ServiceLifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetimeAttribute.ServiceLifetime));
                    }

                    // switch (serviceLifetimeAttribute.ServiceLifetime)
                    // {
                    //    case ServiceLifetime.Singleton:
                    //        services.AddSingleton(Lazy<serviceType>);
                    //        break;
                    //    case ServiceLifetime.Scoped:
                    //        services.AddScoped(serviceType);
                    //        break;
                    //    case ServiceLifetime.Transient:
                    //        services.AddTransient(serviceType);
                    //        break;
                    // }
                    if (serviceType == typeof(IEntityContext))
                    {
                        var arguments = implementationType?.BaseType?.GetGenericArguments();
                        if (arguments is not null)
                        {
                            AddStores(services, arguments[1], arguments[2], arguments[0]);
                        }
                    }
                }

                if (identity)
                {
                    if (serviceType == typeof(IEntityContext))
                    {
                        services
                            .AddIdentity<TUser, TRole>(options => { Configuration.Bind("IdentityOptions", options); })
                            .AddDefaultTokenProviders()
                            .AddErrorDescriber<LocalizedIdentityErrorDescriber>();
                    }
                }
            }

            var config = new TypeAdapterConfig();
            config.Scan(assemblies.ToArray());
            services.AddSingleton(config);
        }
    }
}

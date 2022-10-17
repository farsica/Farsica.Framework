namespace Farsica.Framework.Localization
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using Farsica.Framework.Core;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ResourceManagerStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache resourceNamesCache = new ResourceNamesCache();
        private readonly ConcurrentDictionary<string, ResourceManagerStringLocalizer> localizerCache = new();
        private readonly string resourcesRelativePath;
        private readonly ILoggerFactory loggerFactory;

        public ResourceManagerStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }

            resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (!string.IsNullOrEmpty(resourcesRelativePath))
            {
                resourcesRelativePath = resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            var typeInfo = resourceSource.GetTypeInfo();
            var baseName = GetResourcePrefix(typeInfo).PrepareResourcePath();

            var assemblyName = typeInfo.Assembly.FullName.PrepareResourcePath();
            var assembly = Assembly.Load(assemblyName);

            return localizerCache.GetOrAdd(baseName, _ => CreateResourceManagerStringLocalizer(assembly, baseName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            baseName = GetResourcePrefix(baseName, location).PrepareResourcePath();
            location = location.PrepareResourcePath();

            return localizerCache.GetOrAdd($"B={baseName},L={location}", _ =>
            {
                var assemblyName = new AssemblyName(location);
                var assembly = Assembly.Load(assemblyName);

                return CreateResourceManagerStringLocalizer(assembly, baseName);
            });
        }

        protected virtual string GetResourcePrefix(TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            return GetResourcePrefix(typeInfo, GetRootNamespace(typeInfo.Assembly), GetResourcePath(typeInfo.Assembly));
        }

        protected virtual string GetResourcePrefix(TypeInfo typeInfo, string baseNamespace, string resourcesRelativePath)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            if (string.IsNullOrEmpty(baseNamespace))
            {
                throw new ArgumentNullException(nameof(baseNamespace));
            }

            if (string.IsNullOrEmpty(typeInfo.FullName))
            {
                throw new ArgumentException(nameof(TypeInfo));
            }

            if (string.IsNullOrEmpty(resourcesRelativePath))
            {
                return typeInfo.FullName;
            }
            else
            {
                // This expectation is defined by dotnet's automatic resource storage.
                // We have to conform to "{RootNamespace}.{ResourceLocation}.{FullTypeName - RootNamespace}".
                return baseNamespace + "." + resourcesRelativePath + TrimPrefix(typeInfo.FullName, baseNamespace + ".");
            }
        }

        protected virtual string GetResourcePrefix(string baseResourceName, string baseNamespace)
        {
            if (string.IsNullOrEmpty(baseResourceName))
            {
                throw new ArgumentNullException(nameof(baseResourceName));
            }

            if (string.IsNullOrEmpty(baseNamespace))
            {
                throw new ArgumentNullException(nameof(baseNamespace));
            }

            var assemblyName = new AssemblyName(baseNamespace);
            var assembly = Assembly.Load(assemblyName);
            var rootNamespace = GetRootNamespace(assembly);
            var resourceLocation = GetResourcePath(assembly);
            var locationPath = rootNamespace + "." + resourceLocation;

            baseResourceName = locationPath + TrimPrefix(baseResourceName, baseNamespace + ".");

            return baseResourceName;
        }

        protected virtual ResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, string baseName)
        {
            return new ResourceManagerStringLocalizer(
                new ResourceManager(baseName, assembly),
                assembly,
                baseName,
                resourceNamesCache,
                loggerFactory.CreateLogger<ResourceManagerStringLocalizer>());
        }

        protected virtual string GetResourcePrefix(string location, string baseName, string resourceLocation)
        {
            // Re-root the base name if a resources path is set
            return location + "." + resourceLocation + TrimPrefix(baseName, location + ".");
        }

        protected virtual ResourceLocationAttribute GetResourceLocationAttribute(Assembly assembly)
        {
            return assembly.GetCustomAttribute<ResourceLocationAttribute>();
        }

        protected virtual RootNamespaceAttribute GetRootNamespaceAttribute(Assembly assembly)
        {
            return assembly.GetCustomAttribute<RootNamespaceAttribute>();
        }

        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name[prefix.Length..];
            }

            return name;
        }

        private string GetRootNamespace(Assembly assembly)
        {
            var rootNamespaceAttribute = GetRootNamespaceAttribute(assembly);
            if (rootNamespaceAttribute != null)
            {
                return rootNamespaceAttribute.RootNamespace;
            }

            return assembly.GetName().Name;
        }

        private string GetResourcePath(Assembly assembly)
        {
            var resourceLocationAttribute = GetResourceLocationAttribute(assembly);

            // If we don't have an attribute assume all assemblies use the same resource location.
            var resourceLocation = resourceLocationAttribute == null
                ? resourcesRelativePath
                : resourceLocationAttribute.ResourceLocation + ".";
            resourceLocation = resourceLocation
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            return resourceLocation;
        }
    }
}

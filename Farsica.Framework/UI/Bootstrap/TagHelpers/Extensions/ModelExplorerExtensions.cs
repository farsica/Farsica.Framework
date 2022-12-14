namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Farsica.Framework.Core;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class ModelExplorerExtensions
    {
        public static IEnumerable<Attribute>? GetAttributes([NotNull] this ModelExplorer property)
        {
            return property.Metadata?.PropertyName is null ? null : property.Metadata.ContainerType?.GetTypeInfo()?.GetProperty(property.Metadata.PropertyName)?.GetCustomAttributes();
        }

        public static T? GetAttribute<T>([NotNull] this IEnumerable<Attribute> attributes)
            where T : Attribute
        {
            return attributes.OfType<T>().FirstOrDefault();
        }

        public static T? GetAttribute<T>([NotNull] this ModelExplorer property)
            where T : Attribute
        {
            return property.Metadata.PropertyName is null ? null : property.Metadata?.ContainerType?.GetTypeInfo()?.GetProperty(property.Metadata.PropertyName)?.GetCustomAttribute<T>();
        }

        public static int GetDisplayOrder(this ModelExplorer explorer)
        {
            return GetAttribute<DisplayAttribute>(explorer)?.Order ?? Constants.DisplayOrder;
        }
    }
}

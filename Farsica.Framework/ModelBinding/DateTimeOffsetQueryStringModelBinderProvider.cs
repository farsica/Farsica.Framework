namespace Farsica.Framework.ModelBinding
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DateTimeOffsetQueryStringModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var fullyQualifiedAssemblyName = context.Metadata.ModelType.FullName;
            if (fullyQualifiedAssemblyName is null)
            {
                return null;
            }

            var type = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);
            if (type is null)
            {
                return null;
            }

            var dateTimeOffsetType = typeof(DateTimeOffset);
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            {
                dateTimeOffsetType = type.GenericTypeArguments[0];
            }

            if (type.IsSubclassOf(dateTimeOffsetType) is false)
            {
                return null;
            }

            return Activator.CreateInstance(typeof(DateTimeOffsetQueryStringModelBinder).MakeGenericType(type)) as IModelBinder;
        }
    }
}

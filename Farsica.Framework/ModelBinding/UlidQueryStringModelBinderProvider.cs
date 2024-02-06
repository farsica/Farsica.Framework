namespace Farsica.Framework.ModelBinding
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class UlidQueryStringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

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

            var typeOfUlid = typeof(string);
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            {
                typeOfUlid = type.GenericTypeArguments[0];
            }

            if (type.IsSubclassOf(typeOfUlid) is false)
            {
                return null;
            }

            return Activator.CreateInstance(typeof(UlidQueryStringModelBinder)) as IModelBinder;
        }
    }
}

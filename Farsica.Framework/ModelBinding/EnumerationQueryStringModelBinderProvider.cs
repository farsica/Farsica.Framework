namespace Farsica.Framework.ModelBinding
{
    using System;
    using Farsica.Framework.Data.Enumeration;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var fullyQualifiedAssemblyName = context.Metadata.ModelType.FullName;
            if (fullyQualifiedAssemblyName is null)
            {
                return null;
            }

            var enumType = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);

            if (enumType is null || !enumType.IsSubclassOf(typeof(Enumeration)))
            {
                return null;
            }

            // var methodInfo = typeof(EnumerationQueryStringModelBinder).GetMethod(nameof(EnumerationQueryStringModelBinder.CreateInstance), BindingFlags.Static | BindingFlags.Public);
            // if (methodInfo is null)
            // {
            //    throw new InvalidOperationException("Invalid operation");
            // }

            // var genericMethod = methodInfo.MakeGenericMethod(enumType);
            // return genericMethod.Invoke(null, null) as IModelBinder;
            return Activator.CreateInstance(typeof(EnumerationQueryStringModelBinder<>).MakeGenericType(enumType)) as IModelBinder;
        }
    }
}

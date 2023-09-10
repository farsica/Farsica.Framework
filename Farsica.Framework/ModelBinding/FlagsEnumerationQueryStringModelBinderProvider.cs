namespace Farsica.Framework.ModelBinding
{
    using System;
    using Farsica.Framework.Data.Enumeration;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FlagsEnumerationQueryStringModelBinderProvider : IModelBinderProvider
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

            var flagsEnumType = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);
            if (flagsEnumType is null)
            {
                return null;
            }

            var typeOfFlagsEnumeration = typeof(FlagsEnumeration<>);
            if (!flagsEnumType.IsSubclassOf(typeOfFlagsEnumeration))
            {
                return null;
            }

            return Activator.CreateInstance(typeof(FlagsEnumerationQueryStringModelBinder<>).MakeGenericType(flagsEnumType)) as IModelBinder;
        }
    }
}

namespace Farsica.Framework.ModelBinding
{
    using System;
    using System.Threading.Tasks;
    using Farsica.Framework.Data.Enumeration;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    // public static class EnumerationQueryStringModelBinder
    // {
    //    public static EnumerationQueryStringModelBinder<T> CreateInstance<T>()
    //        where T : Enumeration
    //    {
    //        return new EnumerationQueryStringModelBinder<T>();
    //    }
    // }
    public class EnumerationQueryStringModelBinder<T> : IModelBinder
        where T : Enumeration
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var enumerationName = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (enumerationName.FirstValue.TryGetFromValueOrName<T>(out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();

                bindingContext.ModelState.AddModelError(bindingContext.FieldName, $"{enumerationName.FirstValue} is not supported.");
            }

            return Task.CompletedTask;
        }
    }
}

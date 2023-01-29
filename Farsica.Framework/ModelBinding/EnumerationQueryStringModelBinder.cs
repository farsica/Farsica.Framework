namespace Farsica.Framework.ModelBinding
{
    using System;
    using System.Threading.Tasks;
    using Farsica.Framework.Data.Enumeration;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinder<TEnum, TKey> : IModelBinder
        where TEnum : Enumeration<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var enumerationName = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (enumerationName.FirstValue.TryGetFromNameOrValue<TEnum, TKey>(out var result))
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

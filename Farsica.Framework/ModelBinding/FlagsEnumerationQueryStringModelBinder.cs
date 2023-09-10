namespace Farsica.Framework.ModelBinding
{
    using System;
    using System.Threading.Tasks;
    using Farsica.Framework.Data.Enumeration;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FlagsEnumerationQueryStringModelBinder<TFlagsEnum> : IModelBinder
        where TFlagsEnum : FlagsEnumeration<TFlagsEnum>, new()
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var enumerationName = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            TFlagsEnum? result = null;
            foreach (var item in enumerationName)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                var flagsEnum = item.FromName<TFlagsEnum>();
                if (flagsEnum is not null)
                {
                    result = result is null ? flagsEnum : result | flagsEnum;
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor, item, bindingContext.ModelName));
                    return Task.CompletedTask;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}

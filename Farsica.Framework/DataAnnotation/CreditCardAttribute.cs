namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class CreditCardAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-creditcard", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return ValidationResult.Success;
            }

            var attribute = new System.ComponentModel.DataAnnotations.CreditCardAttribute();
            if (value is List<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || attribute.IsValid(t)) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            }

            return attribute.IsValid(value) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}

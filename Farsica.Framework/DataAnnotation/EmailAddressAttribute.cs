namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class EmailAddressAttribute : ValidationAttribute, IClientModelValidator
    {
        public EmailAddressAttribute()
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_EmailAddress);
        }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var attribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || attribute.IsValid(t));
            }

            return attribute.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-email", Data.Error.FormatMessage(msg)));
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Globalization;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class CultureAttribute : ValidationAttribute, IClientModelValidator
    {
        public CultureAttribute()
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Culture);
        }

        public override bool IsValid(object value)
        {
            return !string.IsNullOrEmpty(value?.ToString()) && CultureInfo.GetCultures(CultureTypes.AllCultures).Exists(t => t.Name == value.ToString());
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", "^[a-z]{2}(-[A-Z]{2})*"));
        }
    }
}

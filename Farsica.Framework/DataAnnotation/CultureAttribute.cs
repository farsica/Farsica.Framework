namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class CultureAttribute : ValidationAttribute, IClientModelValidator
    {
        public CultureAttribute()
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Culture);
        }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || cultures.Exists(c => c.Name.Equals(t, System.StringComparison.OrdinalIgnoreCase)));
            }

            return cultures.Exists(t => t.Name == value.ToString());
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", "^[a-z]{2}(-[A-Z]{2})*"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", Data.Error.FormatMessage(msg)));
        }
    }
}

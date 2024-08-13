namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class UrlAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || Uri.TryCreate(t, UriKind.Absolute, out _));
            }

            return Uri.TryCreate(value.ToString(), UriKind.Absolute, out _);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-url", Data.Error.FormatMessage(msg)));
        }
    }
}

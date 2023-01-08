namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class MobileAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string Pattern = "^(09)[0-9]{2}[0-9]{7}$";
        private static readonly Regex Regex = new(Pattern, RegexOptions.Compiled);

        public override bool IsValid(object? value)
        {
            return string.IsNullOrEmpty(value?.ToString()) || Regex.IsMatch(value.ToString());
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", Pattern));
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed partial class MobileAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string Pattern = "^(09)[0-9]{2}[0-9]{7}$";
        private static readonly Regex Regex = MobileRegex();

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || Regex.IsMatch(t));
            }

            return Regex.IsMatch((string)value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", Pattern));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", Data.Error.FormatMessage(msg)));
        }

        [GeneratedRegex(Pattern, RegexOptions.Compiled)]
        private static partial Regex MobileRegex();
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class FileSizeAttribute : ValidationAttribute, IClientModelValidator
    {
        public FileSizeAttribute(long maximumLength)
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileSize);
            MaximumLength = maximumLength;
        }

        public long MaximumLength { get; }

        public long? MaximumTotalLength { get; set; }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is IEnumerable<IFormFile> lst)
            {
                if (MaximumTotalLength.HasValue && lst.Where(t => t is not null).Sum(t => t.Length) > MaximumTotalLength)
                {
                    return false;
                }

                return lst.All(t => t.Length <= MaximumLength);
            }

            if (value is IFormFile file)
            {
                return file.Length <= MaximumLength;
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize-maxsize", MaximumLength.ToString()));

            if (MaximumTotalLength.HasValue)
            {
                _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize-maxsizetotal", MaximumTotalLength.Value.ToString()));
            }
        }

        private string? FormatErrorMessage(string? modelDisplayName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, modelDisplayName, MaximumLength);
        }
    }
}

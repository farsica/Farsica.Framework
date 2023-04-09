namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class FileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        public FileExtensionsAttribute(string[] extensions)
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileExtensions);
            Extensions = extensions;
        }

        public string[] Extensions { get; }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is List<IFormFile> lst)
            {
                return lst.All(t => t is null || Extensions.Any(e => e.Equals(Path.GetExtension(t.FileName).TrimStart('.'), System.StringComparison.OrdinalIgnoreCase)));
            }

            if (value is IFormFile file)
            {
                return Extensions.Any(t => t.Equals(Path.GetExtension(file.FileName).TrimStart('.'), System.StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions-extension", string.Join(",", Extensions)));
        }

        private string? FormatErrorMessage(string? modelDisplayName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, modelDisplayName, string.Join(",", Extensions));
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class FileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
#pragma warning disable CA1019 // Define accessors for attribute arguments
        public FileExtensionsAttribute(string extensions)
#pragma warning restore CA1019 // Define accessors for attribute arguments
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileExtensions);
            Extensions = extensions.Split(Constants.JoinDelimiter, System.StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] Extensions { get; }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is IEnumerable<IFormFile> lst)
            {
                return lst.All(t => t is null || Extensions.Exists(e => e.Equals(Path.GetExtension(t.FileName).TrimStart('.'), System.StringComparison.OrdinalIgnoreCase)));
            }

            if (value is IFormFile file)
            {
                return Extensions.Exists(t => t.Equals(Path.GetExtension(file.FileName).TrimStart('.'), System.StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions", FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions-extension", string.Join(",", Extensions)));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(",", Extensions));
        }
    }
}

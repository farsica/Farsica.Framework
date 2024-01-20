namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.AspNetCore.StaticFiles;
    using MimeDetective;
    using MimeDetective.Storage;

    public sealed class FileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly Lazy<(ContentInspector ContentInspector, FileExtensionContentTypeProvider ContentTypeProvider)> inspector;

#pragma warning disable CA1019 // Define accessors for attribute arguments
        public FileExtensionsAttribute(string extensions)
#pragma warning restore CA1019 // Define accessors for attribute arguments
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileExtensions);
            Extensions = extensions.Split(Constants.JoinDelimiter, StringSplitOptions.RemoveEmptyEntries);

            inspector = new(() =>
            {
                var definitions = new MimeDetective.Definitions.ExhaustiveBuilder
                {
                    UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial,
                }.Build();

                var contentInspector = new ContentInspectorBuilder
                {
                    Definitions = definitions.ScopeExtensions(Extensions!).TrimMeta().TrimDescription().ToImmutableArray(),
                    Parallel = true,
                }.Build();

                return (contentInspector, new FileExtensionContentTypeProvider());
            });
        }

        public string[]? Extensions { get; private set; }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is IEnumerable<IFormFile> lst)
            {
                foreach (var item in lst)
                {
                    if (!Extensions!.Exists(t => t.Equals(Path.GetExtension(item.FileName).TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }

                    _ = inspector.Value.ContentTypeProvider.TryGetContentType(item.FileName, out var contentType);
                    if (item.ContentType != contentType)
                    {
                        return false;
                    }

                    var results = inspector.Value.ContentInspector.Inspect(item.OpenReadStream());
                    if (!results.ByFileExtension().Exists(t => Extensions!.Contains(t.Extension)))
                    {
                        return false;
                    }
                }

                return true;
            }

            if (value is IFormFile file)
            {
                if (!Extensions!.Exists(t => t.Equals(Path.GetExtension(file.FileName).TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                _ = inspector.Value.ContentTypeProvider.TryGetContentType(file.FileName, out var contentType);
                if (file.ContentType != contentType)
                {
                    return false;
                }

                var results = inspector.Value.ContentInspector.Inspect(file.OpenReadStream());
                if (!results.ByFileExtension().Exists(t => Extensions!.Contains(t.Extension)))
                {
                    return false;
                }

                return true;
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

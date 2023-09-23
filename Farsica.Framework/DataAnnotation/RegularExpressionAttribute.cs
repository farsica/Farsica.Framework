namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class RegularExpressionAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientModelValidator
    {
        public RegularExpressionAttribute(string pattern)
            : base(pattern)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        public new Type? ErrorMessageResourceType
        {
            get
            {
                return base.ErrorMessageResourceType;
            }

            private set
            {
                base.ErrorMessageResourceType = value;
            }
        }

        public new string? ErrorMessageResourceName
        {
            get
            {
                return base.ErrorMessageResourceName;
            }

            private set
            {
                base.ErrorMessageResourceName = value;
            }
        }

        public new string? ErrorMessage
        {
            get
            {
                return base.ErrorMessage;
            }

            internal set
            {
                base.ErrorMessage = value;
            }
        }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || base.IsValid(t));
            }

            return base.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", Pattern));
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute, IClientModelValidator
    {
        public RangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
        }

        public RangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
        }

        public RangeAttribute(Type type, string? minimum, string? maximum)
            : base(type, minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
            Type = type;
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

        public Type Type { get; }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            return base.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-range", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-max", Maximum.ToString()));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-min", Minimum.ToString()));
        }

        private string? FormatErrorMessage(string? modelDisplayName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, modelDisplayName, Minimum, Maximum);
        }
    }
}

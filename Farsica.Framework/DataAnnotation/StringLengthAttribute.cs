﻿namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute, IClientModelValidator
    {
        public StringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_StringLength);
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
        }

        public StringLengthAttribute(int maximumLength, int minimumLength)
            : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_StringLength);
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
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

            if (value is List<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || base.IsValid(t));
            }

            return base.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length", Data.Error.FormatMessage(msg)));

            if (MaximumLength != int.MaxValue)
            {
                context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length-max", MaximumLength.ToString()));
            }

            if (MinimumLength != 0)
            {
                context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length-min", MinimumLength.ToString()));
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinimumLength, MaximumLength);
        }
    }
}

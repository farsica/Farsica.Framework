﻿namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Farsica.Framework.Core;

    public sealed class RequiredAnotherAttribute : ValidationAttribute
    {
        public RequiredAnotherAttribute([NotNull] string otherProperty)
            : base(() => "ValidationError")
        {
            ArgumentNullException.ThrowIfNull(otherProperty, nameof(otherProperty));

            OtherProperty = otherProperty;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnother);
        }

        public RequiredAnotherAttribute([NotNull] string otherProperty, short minCountOtherProperty, short maxCountOtherProperty)
            : base(() => "ValidationError")
        {
            ArgumentNullException.ThrowIfNull(otherProperty, nameof(otherProperty));

            OtherProperty = otherProperty;
            MinCountOtherProperty = minCountOtherProperty;
            MaxCountOtherProperty = maxCountOtherProperty;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnotherList);
        }

        public string OtherProperty { get; }

        public short? MinCountOtherProperty { get; }

        public short? MaxCountOtherProperty { get; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return null;
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);
            if (otherValue is null)
            {
                var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                return new ValidationResult(string.Format(Resources.GlobalResource.Validation_RequiredAnother, otherDisplayName));
            }

            if (!MaxCountOtherProperty.HasValue && !MinCountOtherProperty.HasValue)
            {
                return null;
            }

            if (otherValue is not IEnumerable<object> listValue || listValue.Count() < MinCountOtherProperty || listValue.Count() > MaxCountOtherProperty)
            {
                var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName!));
                var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);

                return new ValidationResult(string.Format(Resources.GlobalResource.Validation_RequiredAnotherList, displayName, otherDisplayName));
            }

            return null;
        }
    }
}

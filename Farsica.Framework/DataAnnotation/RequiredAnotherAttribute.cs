namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using Farsica.Framework.Core;

    public sealed class RequiredAnotherAttribute : ValidationAttribute
    {
        public RequiredAnotherAttribute(string otherProperty)
            : base(() => "ValidationError")
        {
            SetFields(otherProperty, null, null);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnother);
        }

        public RequiredAnotherAttribute(string otherProperty, short minCountOtherProperty, short maxCountOtherProperty)
            : base(() => "ValidationError")
        {
            SetFields(otherProperty, minCountOtherProperty, maxCountOtherProperty);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnotherList);
        }

        public string? OtherProperty { get; private set; }

        public short? MinCountOtherProperty { get; private set; }

        public short? MaxCountOtherProperty { get; private set; }

        public new string? ErrorMessageResourceName { get; } = nameof(Resources.GlobalResource.Validation_RequiredAnother);

        public new Type ErrorMessageResourceType { get; } = typeof(Resources.GlobalResource);

        public string? FormatErrorMessage(string name, string? otherName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, otherName, MinCountOtherProperty, MaxCountOtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            ValidationResult result = null;
            if (value is IComparable firstComparable)
            {
                var ov = otherValue as IEnumerable<object>;
                if (otherValue is null
                    || (MaxCountOtherProperty.HasValue && (ov.Count() < MinCountOtherProperty || MaxCountOtherProperty < ov.Count())))
                {
                    var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName));
                    var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                    result = new ValidationResult(FormatErrorMessage(displayName, otherDisplayName));
                }
            }

            return result;
        }

        private void SetFields(string otherProperty, short? minCountOtherProperty, short? maxCountOtherProperty)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException(nameof(otherProperty));
            }

            OtherProperty = otherProperty;
            MinCountOtherProperty = minCountOtherProperty ?? 0;
            MaxCountOtherProperty = maxCountOtherProperty;
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Farsica.Framework.Core;

    public sealed class EmptyAnotherAttribute : ValidationAttribute
    {
        public EmptyAnotherAttribute([NotNull] string otherProperty)
            : base(() => "ValidationError")
        {
            ArgumentNullException.ThrowIfNull(otherProperty, nameof(otherProperty));

            OtherProperty = otherProperty;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_EmptyAnother);
        }

        public string OtherProperty { get; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return null;
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);
            if (otherValue is not null)
            {
                var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                return new ValidationResult(string.Format(Resources.GlobalResource.Validation_EmptyAnother, otherDisplayName));
            }

            return null;
        }
    }
}

namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using DynamicExpresso;
    using Farsica.Framework.Core;

    public sealed class DaysDistanceAttribute : ValidationAttribute
    {
        public DaysDistanceAttribute(string otherProperty, int maxDistance, string? expression = null)
            : base(() => "ValidationError")
        {
            OtherProperty = otherProperty;
            MaxDistance = maxDistance;
            Expression = expression;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DaysDistance);
        }

        public string? OtherProperty { get; internal set; }

        public int MaxDistance { get; internal set; }

        public string? Expression { get; internal set; }

        public string? FormatErrorMessage(string name, string? otherName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, otherName, MaxDistance);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Expression))
            {
                var properties = validationContext.ObjectType.GetProperties();
                var interpreter = new Interpreter();
                MaxDistance = interpreter.Eval<int>(Expression, (from info in properties
                                                                 where Expression.Contains(info.Name)
                                                                 select
                                                                 new Parameter(info.Name, info.PropertyType, info.GetValue(validationContext.ObjectInstance, null)))
                    .ToArray());
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = (DateTime)otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName));
            var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);

            return ((DateTime)value - otherValue).TotalDays > MaxDistance
                ? new ValidationResult(FormatErrorMessage(displayName, otherDisplayName))
                : null;
        }
    }
}

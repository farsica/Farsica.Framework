namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class ValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        protected ValidationAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        protected ValidationAttribute(string errorMessage)
            : base(errorMessage)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        protected ValidationAttribute(Func<string> errorMessageAccessor)
            : base(errorMessageAccessor)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        public new Type ErrorMessageResourceType
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

            internal set
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

            private set
            {
                base.ErrorMessage = value;
            }
        }

        protected ValidationContext ValidationContext { get; }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            return base.IsValid(value);
        }
    }
}

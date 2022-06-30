namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute, IClientModelValidator
    {
        public RequiredAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Required);
        }

        public new string ErrorMessageResourceName
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

        public new string ErrorMessage
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

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            if (context.ModelMetadata.ContainerType is not null)
            {
                // var name = context.ModelMetadata.ContainerType.AssemblyQualifiedName.PrepareResourcePath();
                // if (string.IsNullOrEmpty(name) is false)
                // {
                //    var type = Type.GetType(name);
                //    if (type is not null)
                //    {
                //        manager = new System.Resources.ResourceManager(type);
                //    }
                // }
            }

            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-required", FormatErrorMessage(Core.Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
        }
    }
}

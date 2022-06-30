namespace Farsica.Framework.DataAnnotation
{
    using System;

    public sealed class DataTypeAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute
    {
        private DisplayFormatAttribute displayFormat;

        public DataTypeAttribute(ElementDataType elementDataType)
            : base(elementDataType.ToString())
        {
            ElementDataType = elementDataType;

            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DataType);

            switch (elementDataType)
            {
                case ElementDataType.Date:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:d}",
                        ApplyFormatInEditMode = true,
                    };
                    break;
                case ElementDataType.Time:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:t}",
                        ApplyFormatInEditMode = true,
                    };
                    break;
                case ElementDataType.Currency:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:C}",
                    };
                    break;
            }
        }

        public ElementDataType ElementDataType { get; }

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

        public new DisplayFormatAttribute DisplayFormat
        {
            get
            {
                return displayFormat;
            }

            private set
            {
                base.DisplayFormat = displayFormat = value;
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

        public override string GetDataTypeName()
        {
            return ElementDataType.ToString();
        }
    }
}

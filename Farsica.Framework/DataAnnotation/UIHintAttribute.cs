namespace Farsica.Framework.DataAnnotation
{
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Form;

    public sealed class UIHintAttribute : System.ComponentModel.DataAnnotations.UIHintAttribute
    {
        public UIHintAttribute()
            : base(null)
        {
        }

        public LabelPosition LabelPosition { get; set; } = LabelPosition.Default;

        public int Rows { get; set; }

        public int Cols { get; set; }

        public bool Disabled { get; set; }

        public bool Readonly { get; set; }

        public bool Inline { get; set; }

        public FormControlSize Size { get; set; } = FormControlSize.Default;

        public int Grid { get; set; }
    }
}

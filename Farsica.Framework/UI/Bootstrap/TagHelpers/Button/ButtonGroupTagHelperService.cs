namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ButtonGroupTagHelperService : TagHelperService<ButtonGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddButtonGroupClass(context, output);
            AddSizeClass(context, output);
            AddAttributes(context, output);

            output.TagName = "div";
        }

        protected virtual void AddSizeClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Size)
            {
                case ButtonGroupSize.Default:
                    break;
                case ButtonGroupSize.Small:
                    output.Attributes.AddClass("btn-group-sm");
                    break;
                case ButtonGroupSize.Medium:
                    output.Attributes.AddClass("btn-group-md");
                    break;
                case ButtonGroupSize.Large:
                    output.Attributes.AddClass("btn-group-lg");
                    break;
            }
        }

        protected virtual void AddButtonGroupClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Direction)
            {
                case ButtonGroupDirection.Horizontal:
                    output.Attributes.AddClass("btn-group");
                    break;
                case ButtonGroupDirection.Vertical:
                    output.Attributes.AddClass("btn-group-vertical");
                    break;
                default:
                    output.Attributes.AddClass("btn-group");
                    break;
            }
        }

        protected virtual void AddAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("role", "group");
        }
    }
}

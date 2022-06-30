namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class DropdownMenuTagHelperService : TagHelperService<DropdownMenuTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("dropdown-menu");
            output.TagMode = TagMode.StartTagAndEndTag;

            SetAlign(context, output);
        }

        protected virtual void SetAlign(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Align)
            {
                case DropdownAlign.Right:
                    output.Attributes.AddClass("dropdown-menu-right");
                    return;
                case DropdownAlign.Left:
                    return;
            }
        }
    }
}
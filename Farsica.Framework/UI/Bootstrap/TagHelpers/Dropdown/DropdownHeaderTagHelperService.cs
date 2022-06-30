namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class DropdownHeaderTagHelperService : TagHelperService<DropdownHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h6";
            output.Attributes.AddClass("dropdown-header");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
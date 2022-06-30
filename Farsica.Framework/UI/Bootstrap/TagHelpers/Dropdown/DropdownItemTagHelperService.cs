namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class DropdownItemTagHelperService : TagHelperService<DropdownItemTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.AddClass("dropdown-item");
            output.TagMode = TagMode.StartTagAndEndTag;

            SetActiveClassIfActive(context, output);
            SetDisabledClassIfDisabled(context, output);
        }

        protected virtual void SetActiveClassIfActive(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Active ?? false)
            {
                output.Attributes.AddClass("active");
            }
        }

        protected virtual void SetDisabledClassIfDisabled(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Disabled ?? false)
            {
                output.Attributes.AddClass("disabled");
            }
        }
    }
}
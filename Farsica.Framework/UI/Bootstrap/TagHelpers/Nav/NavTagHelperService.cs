namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class NavTagHelperService : TagHelperService<NavTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("nav");
            SetAlign(context, output);
            SetNavStyle(context, output);
        }

        protected virtual void SetAlign(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Align == NavAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("justify-content-" + TagHelper.Align.ToString().ToLowerInvariant());
        }

        protected virtual void SetNavStyle(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.NavStyle)
            {
                case NavStyle.Default:
                    return;
                case NavStyle.Pill:
                    output.Attributes.AddClass("nav-pills");
                    break;
                case NavStyle.Vertical:
                    output.Attributes.AddClass("flex-column");
                    break;
                case NavStyle.PillVertical:
                    output.Attributes.AddClass("nav-pills");
                    output.Attributes.AddClass("flex-column");
                    break;
            }
        }
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class NavbarTextTagHelperService : TagHelperService<NavbarTextTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("navbar-text");
            output.Attributes.RemoveAll("frb-navbar-text");
        }
    }
}
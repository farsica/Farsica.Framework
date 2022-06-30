namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar")]
    public class NavBarTagHelper : TagHelper<NavBarTagHelper, NavBarTagHelperService>
    {
        public NavBarTagHelper(NavBarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        public NavbarSize Size { get; set; } = NavbarSize.Default;

        public NavbarStyle NavbarStyle { get; set; } = NavbarStyle.Default;
    }
}

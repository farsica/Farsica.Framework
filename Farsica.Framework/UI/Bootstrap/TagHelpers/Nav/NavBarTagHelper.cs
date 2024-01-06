namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar")]
    public class NavBarTagHelper(NavBarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<NavBarTagHelper, NavBarTagHelperService>(tagHelperService, optionsAccessor)
    {
        public NavbarSize Size { get; set; } = NavbarSize.Default;

        public NavbarStyle NavbarStyle { get; set; } = NavbarStyle.Default;
    }
}

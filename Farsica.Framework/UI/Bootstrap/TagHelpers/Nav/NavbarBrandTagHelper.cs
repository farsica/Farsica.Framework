namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-navbar-brand")]
    public class NavbarBrandTagHelper(NavbarBrandTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<NavbarBrandTagHelper, NavbarBrandTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

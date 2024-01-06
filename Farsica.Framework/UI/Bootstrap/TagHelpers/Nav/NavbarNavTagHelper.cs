namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar-nav")]
    public class NavbarNavTagHelper(NavbarNavTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<NavbarNavTagHelper, NavbarNavTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

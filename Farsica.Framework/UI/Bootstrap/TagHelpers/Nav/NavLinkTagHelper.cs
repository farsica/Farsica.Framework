namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav-link")]
    public class NavLinkTagHelper(NavLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<NavLinkTagHelper, NavLinkTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }
    }
}

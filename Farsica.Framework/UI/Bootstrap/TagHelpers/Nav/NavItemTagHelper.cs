namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav-item")]
    public class NavItemTagHelper : TagHelper<NavItemTagHelper, NavItemTagHelperService>
    {
        public NavItemTagHelper(NavItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-dropdown")]
        public bool? Dropdown { get; set; }
    }
}

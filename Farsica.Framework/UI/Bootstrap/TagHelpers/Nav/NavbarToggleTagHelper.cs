namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar-toggle")]
    public class NavbarToggleTagHelper : TagHelper<NavbarToggleTagHelper, NavbarToggleTagHelperService>
    {
        public NavbarToggleTagHelper(NavbarToggleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-id")]
        public string? Id { get; set; }
    }
}

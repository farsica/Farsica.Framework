namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Badge
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-badge")]
    [HtmlTargetElement("span", Attributes = "frb-badge")]
    [HtmlTargetElement("a", Attributes = "frb-badge-pill")]
    [HtmlTargetElement("span", Attributes = "frb-badge-pill")]
    public class BadgeTagHelper(BadgeTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<BadgeTagHelper, BadgeTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-badge")]
        public BadgeType BadgeType { get; set; } = BadgeType._;

        [HtmlAttributeName("frb-badge-pill")]
        public BadgeType BadgePillType { get; set; } = BadgeType._;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tooltip
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("button", Attributes = "frb-tooltip")]
    [HtmlTargetElement("button", Attributes = "frb-tooltip-right")]
    [HtmlTargetElement("button", Attributes = "frb-tooltip-left")]
    [HtmlTargetElement("button", Attributes = "frb-tooltip-top")]
    [HtmlTargetElement("button", Attributes = "frb-tooltip-bottom")]
    [HtmlTargetElement("frb-button", Attributes = "frb-tooltip")]
    [HtmlTargetElement("frb-button", Attributes = "frb-tooltip-right")]
    [HtmlTargetElement("frb-button", Attributes = "frb-tooltip-left")]
    [HtmlTargetElement("frb-button", Attributes = "frb-tooltip-top")]
    [HtmlTargetElement("frb-button", Attributes = "frb-tooltip-bottom")]
    public class TooltipTagHelper : TagHelper<TooltipTagHelper, TooltipTagHelperService>
    {
        public TooltipTagHelper(TooltipTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-tooltip")]
        public string? Tooltip { get; set; }

        [HtmlAttributeName("frb-tooltip-right")]
        public string? TooltipRight { get; set; }

        [HtmlAttributeName("frb-tooltip-left")]
        public string? TooltipLeft { get; set; }

        [HtmlAttributeName("frb-tooltip-top")]
        public string? TooltipTop { get; set; }

        [HtmlAttributeName("frb-tooltip-bottom")]
        public string? TooltipBottom { get; set; }

        [HtmlAttributeName("frb-title")]
        public string? Title { get; set; }
    }
}

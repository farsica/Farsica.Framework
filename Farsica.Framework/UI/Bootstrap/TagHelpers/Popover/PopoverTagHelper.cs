namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Popover
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("button", Attributes = "frb-popover")]
    [HtmlTargetElement("button", Attributes = "frb-popover-right")]
    [HtmlTargetElement("button", Attributes = "frb-popover-left")]
    [HtmlTargetElement("button", Attributes = "frb-popover-top")]
    [HtmlTargetElement("button", Attributes = "frb-popover-bottom")]
    [HtmlTargetElement("frb-button", Attributes = "frb-popover")]
    [HtmlTargetElement("frb-button", Attributes = "frb-popover-right")]
    [HtmlTargetElement("frb-button", Attributes = "frb-popover-left")]
    [HtmlTargetElement("frb-button", Attributes = "frb-popover-top")]
    [HtmlTargetElement("frb-button", Attributes = "frb-popover-bottom")]
    [HtmlTargetElement("frb-popover")]
    public class PopoverTagHelper : TagHelper<PopoverTagHelper, PopoverTagHelperService>
    {
        public PopoverTagHelper(PopoverTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        [HtmlAttributeName("frb-dismissible")]
        public bool? Dismissible { get; set; }

        [HtmlAttributeName("frb-hoverable")]
        public bool? Hoverable { get; set; }

        [HtmlAttributeName("frb-popover")]
        public string Popover { get; set; }

        [HtmlAttributeName("frb-popover-right")]
        public string PopoverRight { get; set; }

        [HtmlAttributeName("frb-popover-left")]
        public string PopoverLeft { get; set; }

        [HtmlAttributeName("frb-popover-top")]
        public string PopoverTop { get; set; }

        [HtmlAttributeName("frb-popover-bottom")]
        public string PopoverBottom { get; set; }
    }
}

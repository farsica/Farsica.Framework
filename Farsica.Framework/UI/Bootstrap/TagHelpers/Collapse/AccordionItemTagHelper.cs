namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-accordion-item")]
    public class AccordionItemTagHelper(AccordionItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<AccordionItemTagHelper, AccordionItemTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-id")]
        public string? Id { get; set; }

        [HtmlAttributeName("frb-title")]
        public string? Title { get; set; }

        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }
    }
}

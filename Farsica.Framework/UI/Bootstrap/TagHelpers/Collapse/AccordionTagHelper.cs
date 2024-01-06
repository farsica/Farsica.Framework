namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-accordion")]
    public class AccordionTagHelper(AccordionTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<AccordionTagHelper, AccordionTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-id")]
        public string? Id { get; set; }
    }
}

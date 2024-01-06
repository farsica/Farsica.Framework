namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button", Attributes = "frb-collapse-id")]
    [HtmlTargetElement("a", Attributes = "frb-collapse-id")]
    [HtmlTargetElement("frb-collapse-button")]
    public class CollapseButtonTagHelper(CollapseButtonTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CollapseButtonTagHelper, CollapseButtonTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-collapse-id")]
        public string? BodyId { get; set; }
    }
}

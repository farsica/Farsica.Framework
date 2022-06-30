namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button", Attributes = "frb-collapse-id")]
    [HtmlTargetElement("a", Attributes = "frb-collapse-id")]
    [HtmlTargetElement("frb-collapse-button")]
    public class CollapseButtonTagHelper : TagHelper<CollapseButtonTagHelper, CollapseButtonTagHelperService>
    {
        public CollapseButtonTagHelper(CollapseButtonTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-collapse-id")]
        public string BodyId { get; set; }
    }
}

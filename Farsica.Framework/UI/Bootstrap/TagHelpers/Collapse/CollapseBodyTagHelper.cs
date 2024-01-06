namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-collapse-body")]
    public class CollapseBodyTagHelper(CollapseBodyTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CollapseBodyTagHelper, CollapseBodyTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-id")]
        public string? Id { get; set; }

        [HtmlAttributeName("frb-multi")]
        public bool? Multi { get; set; }

        [HtmlAttributeName("frb-show")]
        public bool? Show { get; set; }
    }
}

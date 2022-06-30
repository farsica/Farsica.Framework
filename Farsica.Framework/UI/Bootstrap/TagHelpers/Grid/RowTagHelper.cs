namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Grid
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-row")]
    [HtmlTargetElement("frb-form-row")]
    public class RowTagHelper : TagHelper<RowTagHelper, RowTagHelperService>
    {
        public RowTagHelper(RowTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-vertical-align")]
        public VerticalAlign VerticalAlign { get; set; } = VerticalAlign.Default;

        [HtmlAttributeName("frb-horizontal-align")]
        public HorizontalAlign HorizontalAlign { get; set; } = HorizontalAlign.Default;

        [HtmlAttributeName("frb-gutters")]
        public bool? Gutters { get; set; } = true;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-grid-view-column", ParentTag = "frb-grid-view-column")]
    public class GridViewColumnTagHelper(GridViewColumnTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<GridViewColumnTagHelper, GridViewColumnTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-entity-type")]
        public string? EntityType { get; set; }
    }
}

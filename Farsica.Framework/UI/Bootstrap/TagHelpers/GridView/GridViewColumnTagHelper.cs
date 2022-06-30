namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-grid-view-column", ParentTag = "frb-grid-view-column")]
    public class GridViewColumnTagHelper : TagHelper<GridViewColumnTagHelper, GridViewColumnTagHelperService>
    {
        #region Constructors

        public GridViewColumnTagHelper(GridViewColumnTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        #endregion

        [HtmlAttributeName("frb-entity-type")]
        public string EntityType { get; set; }
    }
}

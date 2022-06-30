namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-grid-view-columns", ParentTag = "frb-grid-view")]
    public class GridViewColumnsTagHelper : TagHelper<GridViewColumnsTagHelper, GridViewColumnsTagHelperService>
    {
        #region Constructors

        public GridViewColumnsTagHelper(GridViewColumnsTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        #endregion
    }
}

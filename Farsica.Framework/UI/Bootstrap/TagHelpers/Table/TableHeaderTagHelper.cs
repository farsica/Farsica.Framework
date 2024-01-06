namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Table
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-table-header")]
    public class TableHeaderTagHelper(TableHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<TableHeaderTagHelper, TableHeaderTagHelperService>(tagHelperService, optionsAccessor)
    {
        public TableHeaderTheme Theme { get; set; } = TableHeaderTheme.Default;
    }
}

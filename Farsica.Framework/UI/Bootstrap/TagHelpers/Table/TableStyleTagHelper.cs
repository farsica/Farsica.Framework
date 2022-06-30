namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Table
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tr")]
    [HtmlTargetElement("frb-td")]
    public class TableStyleTagHelper : TagHelper<TableStyleTagHelper, TableStyleTagHelperService>
    {
        public TableStyleTagHelper(TableStyleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-table-style")]
        public TableStyle TableStyle { get; set; } = TableStyle.Default;
    }
}

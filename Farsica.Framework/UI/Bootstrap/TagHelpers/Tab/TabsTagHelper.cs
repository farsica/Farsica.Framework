namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Grid;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tabs")]
    public class TabsTagHelper(TabsTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<TabsTagHelper, TabsTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-tab-style")]
        public TabStyle TabStyle { get; set; } = TabStyle.Tab;

        [HtmlAttributeName("frb-vertical-header-size")]
        public ColumnSize VerticalHeaderSize { get; set; } = ColumnSize._3;
    }
}

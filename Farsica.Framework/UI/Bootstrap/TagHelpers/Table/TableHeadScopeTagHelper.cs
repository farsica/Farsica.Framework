namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Table
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-th")]
    public class TableHeadScopeTagHelper(TableHeadScopeTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<TableHeadScopeTagHelper, TableHeadScopeTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-scope")]
        public ThScope Scope { get; set; } = ThScope.Default;
    }
}

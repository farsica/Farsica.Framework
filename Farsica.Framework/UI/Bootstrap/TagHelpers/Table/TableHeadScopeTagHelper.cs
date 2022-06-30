namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Table
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-th")]
    public class TableHeadScopeTagHelper : TagHelper<TableHeadScopeTagHelper, TableHeadScopeTagHelperService>
    {
        public TableHeadScopeTagHelper(TableHeadScopeTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-scope")]
        public ThScope Scope { get; set; } = ThScope.Default;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Pagination
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-paginator")]
    public class PaginationTagHelper(PaginationTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<PaginationTagHelper, PaginationTagHelperService>(tagHelperService, optionsAccessor)
    {
        [NotNull]
        [HtmlAttributeName("frb-for")]
        public new PagerModel? For { get; set; }

        [HtmlAttributeName("frb-show-info")]
        public bool? ShowInfo { get; set; }
    }
}

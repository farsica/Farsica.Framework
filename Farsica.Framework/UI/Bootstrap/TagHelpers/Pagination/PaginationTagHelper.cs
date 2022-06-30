namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Pagination
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-paginator")]
    public class PaginationTagHelper : TagHelper<PaginationTagHelper, PaginationTagHelperService>
    {
        public PaginationTagHelper(PaginationTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-for")]
        public new PagerModel For { get; set; }

        [HtmlAttributeName("frb-show-info")]
        public bool? ShowInfo { get; set; }
    }
}

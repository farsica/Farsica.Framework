namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-bread-crumb")]
    public class BreadcrumbTagHelper : TagHelper<BreadcrumbTagHelper, BreadcrumbTagHelperService>
    {
        public BreadcrumbTagHelper(BreadcrumbTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-bread-crumb")]
    public class BreadcrumbTagHelper(BreadcrumbTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<BreadcrumbTagHelper, BreadcrumbTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

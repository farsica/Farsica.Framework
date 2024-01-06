namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Grid
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-column-breaker")]
    public class ColumnBreakerTagHelper(ColumnBreakerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ColumnBreakerTagHelper, ColumnBreakerTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

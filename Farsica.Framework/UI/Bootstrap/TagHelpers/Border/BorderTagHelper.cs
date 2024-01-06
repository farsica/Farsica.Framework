namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-border")]
    public class BorderTagHelper(BorderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<BorderTagHelper, BorderTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-border")]
        public BorderType Border { get; set; } = BorderType.Default;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-rounded")]
    public class RoundedTagHelper(RoundedTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<RoundedTagHelper, RoundedTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-rounded")]
        public RoundedType Rounded { get; set; } = RoundedType.Default;
    }
}

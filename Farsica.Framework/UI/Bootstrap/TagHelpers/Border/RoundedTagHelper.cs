namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-rounded")]
    public class RoundedTagHelper : TagHelper<RoundedTagHelper, RoundedTagHelperService>
    {
        public RoundedTagHelper(RoundedTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-rounded")]
        public RoundedType Rounded { get; set; } = RoundedType.Default;
    }
}

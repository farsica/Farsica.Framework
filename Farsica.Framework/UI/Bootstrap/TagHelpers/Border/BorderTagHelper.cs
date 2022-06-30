namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-border")]
    public class BorderTagHelper : TagHelper<BorderTagHelper, BorderTagHelperService>
    {
        public BorderTagHelper(BorderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-border")]
        public BorderType Border { get; set; } = BorderType.Default;
    }
}

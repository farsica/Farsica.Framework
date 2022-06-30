namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Utils
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-auto-focus")]
    public class AutoFocusTagHelper : TagHelpers.TagHelper
    {
        public AutoFocusTagHelper(IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-auto-focus")]
        public bool AutoFocus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AutoFocus)
            {
                output.Attributes.Add("data-auto-focus", "true");
            }
        }
    }
}

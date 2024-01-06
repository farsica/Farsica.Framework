namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Utils
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-if")]
    public class IfTagHelper(IOptions<MvcViewOptions> optionsAccessor) : TagHelpers.TagHelper(optionsAccessor)
    {
        [HtmlAttributeName("frb-if")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-progress-group")]
    public class ProgressGroupTagHelper : TagHelper<ProgressGroupTagHelper, ProgressGroupTagHelperService>
    {
        public ProgressGroupTagHelper(ProgressGroupTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

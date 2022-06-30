namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-progress-bar")]
    [HtmlTargetElement("frb-progress-part")]
    public class ProgressBarTagHelper : TagHelper<ProgressBarTagHelper, ProgressBarTagHelperService>
    {
        public ProgressBarTagHelper(ProgressBarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-min-value")]
        public double MinValue { get; set; } = 0;

        [HtmlAttributeName("frb-max-value")]
        public double MaxValue { get; set; } = 100;

        [HtmlAttributeName("frb-type")]
        public ProgressBarType Type { get; set; } = ProgressBarType.Default;

        [HtmlAttributeName("frb-strip")]
        public bool? Strip { get; set; }

        [HtmlAttributeName("frb-animation")]
        public bool? Animation { get; set; }
    }
}

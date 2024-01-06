namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Alert
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertTagHelper(AlertTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<AlertTagHelper, AlertTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-alert-type")]
        public AlertType AlertType { get; set; } = AlertType.Default;

        [HtmlAttributeName("frb-dismissible")]
        public bool? Dismissible { get; set; }
    }
}

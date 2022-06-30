namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Alert
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-alert-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertLinkTagHelper : TagHelper<AlertLinkTagHelper, AlertLinkTagHelperService>
    {
        public AlertLinkTagHelper(AlertLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Alert
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-alert-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertLinkTagHelper(AlertLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<AlertLinkTagHelper, AlertLinkTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

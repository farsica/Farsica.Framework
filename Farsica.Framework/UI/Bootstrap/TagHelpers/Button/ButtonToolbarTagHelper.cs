namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button-toolbar")]
    public class ButtonToolbarTagHelper(ButtonToolbarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ButtonToolbarTagHelper, ButtonToolbarTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

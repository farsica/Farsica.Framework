namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-header")]
    public class CardHeaderTagHelper(CardHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardHeaderTagHelper, CardHeaderTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-text")]
    public class CardTextTagHelper(CardTextTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardTextTagHelper, CardTextTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}
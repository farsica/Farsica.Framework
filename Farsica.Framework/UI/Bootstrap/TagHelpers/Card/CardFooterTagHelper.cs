namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-footer")]
    public class CardFooterTagHelper(CardFooterTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardFooterTagHelper, CardFooterTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

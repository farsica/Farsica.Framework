namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-card-link")]
    [HtmlTargetElement("frb-card-link")]
    public class CardLinkTagHelper : TagHelper<CardLinkTagHelper, CardLinkTagHelperService>
    {
        public CardLinkTagHelper(CardLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    public class CardTagHelper(CardTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardTagHelper, CardTagHelperService>(tagHelperService, optionsAccessor)
    {
        public CardBorderColorType Border { get; set; } = CardBorderColorType.Default;
    }
}

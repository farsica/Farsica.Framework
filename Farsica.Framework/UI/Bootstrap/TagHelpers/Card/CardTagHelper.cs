namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    public class CardTagHelper : TagHelper<CardTagHelper, CardTagHelperService>
    {
        public CardTagHelper(CardTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        public CardBorderColorType Border { get; set; } = CardBorderColorType.Default;
    }
}

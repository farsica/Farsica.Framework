namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("img", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("frb-image", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    public class CardImageTagHelper : TagHelper<CardImageTagHelper, CardImageTagHelperService>
    {
        public CardImageTagHelper(CardImageTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-card-image")]
        public CardImagePosition Position { get; set; } = CardImagePosition.Top;
    }
}

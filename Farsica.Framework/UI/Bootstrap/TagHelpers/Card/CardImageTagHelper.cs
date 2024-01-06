namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("img", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("frb-image", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    public class CardImageTagHelper(CardImageTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardImageTagHelper, CardImageTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-card-image")]
        public CardImagePosition Position { get; set; } = CardImagePosition.Top;
    }
}

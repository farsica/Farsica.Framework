namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-subtitle")]
    public class CardSubtitleTagHelper : TagHelper<CardSubtitleTagHelper, CardSubtitleTagHelperService>
    {
        public CardSubtitleTagHelper(CardSubtitleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-default-heading")]
        public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H6;

        [HtmlAttributeName("frb-heading")]
        public HtmlHeadingType Heading { get; set; } = DefaultHeading;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-subtitle")]
    public class CardSubtitleTagHelper(CardSubtitleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardSubtitleTagHelper, CardSubtitleTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-default-heading")]
        public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H6;

        [HtmlAttributeName("frb-heading")]
        public HtmlHeadingType Heading { get; set; } = DefaultHeading;
    }
}

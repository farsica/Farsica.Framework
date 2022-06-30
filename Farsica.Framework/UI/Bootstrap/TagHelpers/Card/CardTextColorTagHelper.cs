namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card", Attributes = "frb-text-color")]
    [HtmlTargetElement("frb-card-header", Attributes = "frb-text-color")]
    [HtmlTargetElement("frb-card-body", Attributes = "frb-text-color")]
    [HtmlTargetElement("frb-card-footer", Attributes = "frb-text-color")]
    public class CardTextColorTagHelper : TagHelper<CardTextColorTagHelper, CardTextColorTagHelperService>
    {
        public CardTextColorTagHelper(CardTextColorTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-text-color")]
        public CardTextColorType TextColor { get; set; } = CardTextColorType.Default;
    }
}

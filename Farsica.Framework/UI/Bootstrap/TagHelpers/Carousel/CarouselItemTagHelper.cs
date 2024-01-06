namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-carousel-item")]
    public class CarouselItemTagHelper(CarouselItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CarouselItemTagHelper, CarouselItemTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-src")]
        public string? Src { get; set; }

        [HtmlAttributeName("frb-alt")]
        public string? Alt { get; set; }

        [HtmlAttributeName("frb-caption-title")]
        public string? CaptionTitle { get; set; }

        [HtmlAttributeName("frb-caption")]
        public string? Caption { get; set; }
    }
}

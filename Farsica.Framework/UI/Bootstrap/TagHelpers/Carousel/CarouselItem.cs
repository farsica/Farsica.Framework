namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    public class CarouselItem(string? html, bool active)
    {
        public string? Html { get; set; } = html;

        public bool Active { get; set; } = active;
    }
}

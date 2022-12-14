namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Encodings.Web;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CarouselItemTagHelperService : TagHelperService<CarouselItemTagHelper>
    {
        private readonly HtmlEncoder encoder;

        public CarouselItemTagHelperService(HtmlEncoder encoder)
        {
            this.encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("carousel-item");
            output.Attributes.AddClass(CarouselItemActivePlaceholder);

            SetInnerImgTag(context, output);
            SetActive(context, output);
            AddCaption(context, output);

            AddToContext(context, output);

            output.SuppressOutput();
        }

        protected virtual void SetInnerImgTag(TagHelperContext context, TagHelperOutput output)
        {
            var imgTag = "<img class=\"d-block w-100\" src=\"" + TagHelper.Src + "\" alt=\"" + TagHelper.Alt + "\">";
            output.Content.SetHtmlContent(imgTag);
        }

        protected virtual void SetActive(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Active ?? false)
            {
                output.Attributes.AddClass("active");
            }
        }

        protected virtual void AddCaption(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(TagHelper.Caption) && string.IsNullOrEmpty(TagHelper.CaptionTitle))
            {
                return;
            }

            var html = new StringBuilder(string.Empty);

            html.AppendLine("<div class=\"carousel-caption d-none d-md-block\">");
            html.AppendLine("<h5>" + TagHelper.CaptionTitle + "</h5>");
            html.AppendLine("<p>" + TagHelper.Caption + "</p>");
            html.AppendLine("</div>");

            output.PostContent.SetHtmlContent(html.ToString());
        }

        private void AddToContext(TagHelperContext context, TagHelperOutput output)
        {
            var getOutputAsHtml = output.Render(encoder);

            var itemList = context.GetValue<List<CarouselItem>>(CarouselItemsContent);

            itemList?.Add(new CarouselItem(getOutputAsHtml, TagHelper.Active ?? false));
        }
    }
}

﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CarouselTagHelperService : TagHelperService<CarouselTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            SetRandomIdIfNotProvided();
            AddBasicAttributes(context, output);

            var itemList = InitilizeCarouselItemsContentsContext(context, output);

            await output.GetChildContentAsync();

            SetOneItemAsActive(context, output, itemList);
            SetItems(context, output, itemList);
            SetControls(context, output, itemList);
            SetIndicators(context, output, itemList);
        }

        protected virtual IList<CarouselItem> InitilizeCarouselItemsContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<CarouselItem>();
            context.Items[CarouselItemsContent] = items;
            return items;
        }

        protected virtual void SetItems(TagHelperContext context, TagHelperOutput output, IList<CarouselItem> itemList)
        {
            var itemsHtml = new StringBuilder(string.Empty);
            itemsHtml.Append("<div class= \"carousel-inner\">");

            foreach (var carouselItem in itemList)
            {
                SetActiveIfActive(carouselItem);

                itemsHtml.AppendLine(carouselItem.Html);
            }

            itemsHtml.Append("</div>");
            output.Content.SetHtmlContent(itemsHtml.ToString());
        }

        protected virtual void SetControls(TagHelperContext context, TagHelperOutput output, IList<CarouselItem> itemList)
        {
            if (!TagHelper.Controls ?? false)
            {
                return;
            }

            var html = new StringBuilder(string.Empty);

            html.AppendLine("<a class=\"carousel-control-prev\" href=\"#" + TagHelper.Id + "\" role=\"button\" data-slide=\"prev\">");
            html.AppendLine("<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span>");
            html.AppendLine("<span class=\"sr-only\">Previous</span>");
            html.AppendLine("</a>");
            html.AppendLine("<a class=\"carousel-control-next\" href=\"#" + TagHelper.Id + "\" role=\"button\" data-slide=\"next\">");
            html.AppendLine("<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span>");
            html.AppendLine("<span class=\"sr-only\">Next</span>");
            html.AppendLine("</a>");

            output.PostContent.SetHtmlContent(html.ToString());
        }

        protected virtual void SetIndicators(TagHelperContext context, TagHelperOutput output, IList<CarouselItem> itemList)
        {
            if (!TagHelper.Indicators ?? false)
            {
                return;
            }

            var html = new StringBuilder("<ol class=\"carousel-indicators\">");

            for (var i = 0; i < itemList.Count; i++)
            {
                html.AppendLine(
                    "<li " +
                    "data-target=\"#" + TagHelper.Id + "\"" +
                    " data-slide-to=\"" + i + "\"" +
                    (itemList[i].Active ? " class=\"active\">" : string.Empty) +
                    "</li>");
            }

            html.AppendLine("</ol>");
            output.PreContent.SetHtmlContent(html.ToString());
        }

        protected virtual void SetOneItemAsActive(TagHelperContext context, TagHelperOutput output, IList<CarouselItem> itemList)
        {
            if (!itemList.Any(t => t.Active) && itemList.Count > 0)
            {
                itemList.First().Active = true;
            }
        }

        protected virtual void AddBasicAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-ride", "carousel");
            output.Attributes.Add("id", TagHelper.Id);
            AddBasicClasses(context, output);
        }

        protected virtual void AddBasicClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("carousel");
            output.Attributes.AddClass("slide");
            SetFadeAnimation(context, output);
        }

        protected virtual void SetFadeAnimation(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Crossfade ?? false)
            {
                output.Attributes.AddClass("carousel-fade");
            }
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (string.IsNullOrEmpty(TagHelper.Id))
            {
                TagHelper.Id = "C" + Guid.NewGuid().ToString("N");
            }
        }

        protected virtual void SetActiveIfActive(CarouselItem item)
        {
            item.Html = item?.Html?.Replace(CarouselItemActivePlaceholder, item.Active ? "active" : string.Empty);
        }
    }
}

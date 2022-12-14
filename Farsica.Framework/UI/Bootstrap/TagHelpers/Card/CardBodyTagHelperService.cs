namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Farsica.Framework.Core.Extensions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CardBodyTagHelperService : TagHelperService<CardBodyTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("card-body");

            ProcessTitle(output);
            ProcessSubtitle(output);
        }

        protected virtual void ProcessTitle(TagHelperOutput output)
        {
            if (!TagHelper.Title.IsNullOrEmpty())
            {
                var cardTitle = new TagBuilder(CardTitleTagHelper.DefaultHeading.ToHtmlTag());
                cardTitle.AddCssClass("card-title");
                cardTitle.InnerHtml.Append(TagHelper?.Title ?? string.Empty);
                output.PreContent.AppendHtml(cardTitle);
            }
        }

        protected virtual void ProcessSubtitle(TagHelperOutput output)
        {
            if (!TagHelper.Subtitle.IsNullOrEmpty())
            {
                var cardSubtitle = new TagBuilder(CardSubtitleTagHelper.DefaultHeading.ToHtmlTag());
                cardSubtitle.AddCssClass("card-subtitle text-muted mb-2");
                cardSubtitle.InnerHtml.Append(TagHelper?.Subtitle ?? string.Empty);
                output.PreContent.AppendHtml(cardSubtitle);
            }
        }
    }
}

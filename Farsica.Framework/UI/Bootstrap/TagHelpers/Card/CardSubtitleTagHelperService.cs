namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CardSubtitleTagHelperService : TagHelperService<CardSubtitleTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = TagHelper.Heading.ToHtmlTag();
            output.Attributes.AddClass("card-subtitle");
        }
    }
}
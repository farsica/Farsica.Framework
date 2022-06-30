namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CardTagHelperService : TagHelperService<CardTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("card");

            SetBorder(context, output);
        }

        protected virtual void SetBorder(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Border == CardBorderColorType.Default)
            {
                return;
            }

            output.Attributes.AddClass("border-" + TagHelper.Border.ToString().ToLowerInvariant());
        }
    }
}
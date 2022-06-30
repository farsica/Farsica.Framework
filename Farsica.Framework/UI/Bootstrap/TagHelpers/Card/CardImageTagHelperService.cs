namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CardImageTagHelperService : TagHelperService<CardImageTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass(TagHelper.Position.ToClassName());
            output.Attributes.RemoveAll("frb-card-image");
        }
    }
}
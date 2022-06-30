namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Figure
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class FigureCaptionTagHelperService : TagHelperService<FigureCaptionTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "figcaption";
            output.Attributes.AddClass("figure-caption");
        }
    }
}
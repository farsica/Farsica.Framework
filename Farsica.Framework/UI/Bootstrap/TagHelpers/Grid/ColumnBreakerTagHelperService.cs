namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Grid
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ColumnBreakerTagHelperService : TagHelperService<ColumnBreakerTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("w-100");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
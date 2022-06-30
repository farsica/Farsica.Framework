namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ProgressGroupTagHelperService : TagHelperService<ProgressGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("progress");
            output.TagName = "div";
        }
    }
}
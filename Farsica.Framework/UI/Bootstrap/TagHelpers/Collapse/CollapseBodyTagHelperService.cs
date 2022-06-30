namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CollapseBodyTagHelperService : TagHelperService<CollapseBodyTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("collapse");
            output.Attributes.Add("id", TagHelper.Id);

            if (TagHelper.Show ?? false)
            {
                output.Attributes.AddClass("show");
            }

            if (TagHelper.Multi ?? false)
            {
                output.Attributes.AddClass("multi-collapse");
            }

            var innerContent = (await output.GetChildContentAsync()).GetContent();

            output.Content.SetHtmlContent(innerContent);
        }
    }
}
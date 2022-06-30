namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ButtonToolbarTagHelperService : TagHelperService<ButtonToolbarTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("btn-toolbar");
            output.Attributes.Add("role", "toolbar");
        }
    }
}

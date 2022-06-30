namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ModalBodyTagHelperService : TagHelperService<ModalBodyTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-body");
        }
    }
}
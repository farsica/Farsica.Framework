namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ModalHeaderTagHelperService : TagHelperService<ModalHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-header");
            output.PreContent.SetHtmlContent(CreatePreContent());
            output.PostContent.SetHtmlContent(CreatePostContent());
        }

        protected virtual string CreatePreContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("    <h5 class=\"modal-title\">" + TagHelper.Title + "</h5>");

            return sb.ToString();
        }

        protected virtual string CreatePostContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.AppendLine("        <span aria-hidden=\"true\">&times;</span>");
            sb.AppendLine("    </button>");

            return sb.ToString();
        }
    }
}
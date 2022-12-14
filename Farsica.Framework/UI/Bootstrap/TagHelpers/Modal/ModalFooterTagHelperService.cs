namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using System.Text;
    using Farsica.Framework.Resources;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ModalFooterTagHelperService : TagHelperService<ModalFooterTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-footer");

            if (TagHelper.Buttons != ModalButtons.None)
            {
                output.PostContent.SetHtmlContent(CreateContent());
            }

            ProcessButtonsAlignment(output);
        }

        protected virtual string? CreateContent()
        {
            var sb = new StringBuilder();

            switch (TagHelper.Buttons)
            {
                case ModalButtons.Cancel:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + UIResource.Cancel + "</button>");
                    break;
                case ModalButtons.Close:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + UIResource.Close + "</button>");
                    break;
                case ModalButtons.Save:
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + UIResource.SavingWithThreeDot + "\"><i class=\"fas fa-check\"></i> <span>" + UIResource.Save + "</span></button>");
                    break;
                case ModalButtons.Save | ModalButtons.Cancel:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + UIResource.Cancel + "</button>");
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + UIResource.SavingWithThreeDot + "\"><i class=\"fas fa-check\"></i> <span>" + UIResource.Save + "</span></button>");
                    break;
                case ModalButtons.Save | ModalButtons.Close:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + UIResource.Close + "</button>");
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + UIResource.SavingWithThreeDot + "\"><i class=\"fas fa-check\"></i> <span>" + UIResource.Save + "</span></button>");
                    break;
            }

            return sb.ToString();
        }

        protected virtual void ProcessButtonsAlignment(TagHelperOutput output)
        {
            if (TagHelper.ButtonAlignment == ButtonsAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("justify-content-" + TagHelper.ButtonAlignment.ToString().ToLowerInvariant());
        }
    }
}
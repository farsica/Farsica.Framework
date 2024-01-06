namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class BreadcrumbItemTagHelperService(HtmlEncoder encoder) : TagHelperService<BreadcrumbItemTagHelper>
    {
        private readonly HtmlEncoder encoder = encoder;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("breadcrumb-item");
            output.Attributes.AddClass(BreadcrumbItemActivePlaceholder);

            var list = context.GetValue<List<BreadcrumbItem>>(BreadcrumbItemsContent);

            output.Content.SetHtmlContent(GetInnerHtml(context, output));

            list?.Add(new BreadcrumbItem
            {
                Html = output.Render(encoder),
                Active = TagHelper.Active,
            });

            output.SuppressOutput();
        }

        protected virtual string? GetInnerHtml(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(TagHelper.Href))
            {
                output.Attributes.Add("aria-current", "page");
                return TagHelper.Title;
            }

            return "<a href=\"" + TagHelper.Href + "\">" + TagHelper.Title + "</a>";
        }
    }
}

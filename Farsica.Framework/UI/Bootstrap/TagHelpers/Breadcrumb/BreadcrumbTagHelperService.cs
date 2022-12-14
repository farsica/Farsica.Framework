namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class BreadcrumbTagHelperService : TagHelperService<BreadcrumbTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var list = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            SetInnerOlTag(context, output);
            SetInnerList(context, output, list);

            output.TagName = "nav";
            output.Attributes.Add("aria-label", "breadcrumb");
        }

        protected virtual void SetInnerOlTag(TagHelperContext context, TagHelperOutput output)
        {
            output.PreContent.SetHtmlContent("<ol class=\"breadcrumb\">");
            output.PostContent.SetHtmlContent("</ol>");
        }

        protected virtual void SetInnerList(TagHelperContext context, TagHelperOutput output, IList<BreadcrumbItem> list)
        {
            SetLastOneActiveIfThereIsNotAny(context, output, list);

            var html = new StringBuilder(string.Empty);

            foreach (var breadcrumbItem in list)
            {
                var htmlPart = SetActiveClassIfActiveAndGetHtml(breadcrumbItem);

                html.AppendLine(htmlPart);
            }

            output.Content.SetHtmlContent(html.ToString());
        }

        protected virtual IList<BreadcrumbItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<BreadcrumbItem>();
            context.Items[BreadcrumbItemsContent] = items;
            return items;
        }

        protected virtual void SetLastOneActiveIfThereIsNotAny(TagHelperContext context, TagHelperOutput output, IList<BreadcrumbItem> list)
        {
            if (list.Count > 0 && !list.Any(bc => bc.Active))
            {
                list.Last().Active = true;
            }
        }

        protected virtual string? SetActiveClassIfActiveAndGetHtml(BreadcrumbItem item)
        {
            return item.Active ?
                item.Html?.Replace(BreadcrumbItemActivePlaceholder, " active") :
                item.Html?.Replace(BreadcrumbItemActivePlaceholder, string.Empty);
        }
    }
}

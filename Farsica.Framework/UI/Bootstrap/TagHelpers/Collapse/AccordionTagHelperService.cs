namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class AccordionTagHelperService : TagHelperService<AccordionTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomIdIfNotProvided();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("accordion");
            output.Attributes.Add("id", TagHelper?.Id);

            var items = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            var content = GetContent(items);

            output.Content.SetHtmlContent(content);
        }

        protected virtual IReadOnlyList<string> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<string>();
            context.Items[AccordionItems] = items;
            return items;
        }

        protected virtual string? GetContent(IReadOnlyList<string> items)
        {
            var html = new StringBuilder(string.Empty);
            foreach (var item in items)
            {
                var content = item.Replace(AccordionParentIdPlaceholder, TagHelper?.Id);

                html.AppendLine(
                    "<div class=\"card\">" + Environment.NewLine +
                        content
                    + "</div>" + Environment.NewLine);
            }

            return html.ToString();
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (TagHelper is not null && string.IsNullOrEmpty(TagHelper.Id))
            {
                TagHelper.Id = "A" + Guid.NewGuid().ToString("N");
            }
        }
    }
}

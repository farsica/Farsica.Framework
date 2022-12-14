namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class AccordionItemTagHelperService : TagHelperService<AccordionItemTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomIdIfNotProvided();

            var innerContent = (await output.GetChildContentAsync()).GetContent();

            var html = GetAccordionHeaderItem(context, output) + GetAccordionContentItem(context, output, innerContent);

            var tabHeaderItems = context.GetValue<List<string>>(AccordionItems);
            tabHeaderItems?.Add(html);

            output.SuppressOutput();
        }

        protected virtual string? GetAccordionHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            return "     <div class=\"card-header\" id=\"" + GetHeadingId() + "\">" + Environment.NewLine +
                   "      <h5 class=\"mb-0\">" + Environment.NewLine +
                   "        <button class=\"btn btn-link\" type=\"button\" data-toggle=\"collapse\" data-target=\"#" + GetContentId() + "\" aria-expanded=\"true\" aria-controls=\"" + GetContentId() + "\">" + Environment.NewLine +
                                    TagHelper.Title + Environment.NewLine +
                   "        </button>" + Environment.NewLine +
                   "      </h5>" + Environment.NewLine +
                   "    </div>" + Environment.NewLine;
        }

        protected virtual string? GetAccordionContentItem(TagHelperContext context, TagHelperOutput output, string? content)
        {
            var show = (TagHelper.Active ?? false) ? " show" : string.Empty;
            return "<div id=\"" + GetContentId() + "\" class=\"collapse" + show + "\" aria-labelledby=\"" + GetHeadingId() + "\" data-parent=\"#" + AccordionParentIdPlaceholder + "\">" + Environment.NewLine +
                   "      <div class=\"card-body\">" + Environment.NewLine +
                            content + Environment.NewLine +
                   "      </div>" + Environment.NewLine +
                   "</div>" + Environment.NewLine;
        }

        protected virtual string? GetHeadingId()
        {
            return "heading" + TagHelper.Id;
        }

        protected virtual string? GetContentId()
        {
            return "content" + TagHelper.Id;
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (string.IsNullOrEmpty(TagHelper.Id))
            {
                TagHelper.Id = "A" + Guid.NewGuid().ToString("N");
            }
        }
    }
}

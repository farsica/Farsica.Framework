namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class TabDropdownTagHelperService : TagHelperService<TabDropdownTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Name))
            {
                throw new Exception("Name of tab dropdown tag can not bu null or empty.");
            }

            await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, string.Empty, false, TagHelper.Name, string.Empty, true));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var title = TagHelper.Title;

            return "<li class=\"nav-item dropdown\">" +
                   "<a class=\"nav-link dropdown-toggle\" id=\"" + id + "\" data-toggle=\"dropdown\" href=\"#" + link + "\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\">" +
                   title +
                   "</a>" +
                   "<div class=\"dropdown-menu\">" +
                   TabDropdownItemsActivePlaceholder +
                   "</div>" +
                   "</li>";
        }
    }
}
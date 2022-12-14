namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class TabLinkTagHelperService : TagHelperService<TabLinkTagHelper>
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();

            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);
            tabHeaderItems?.Add(new TabItem(tabHeader, string.Empty, false, TagHelper?.Name, TagHelper?.ParentDropdownName, false));

            output.SuppressOutput();

            return Task.CompletedTask;
        }

        protected virtual string? GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper?.Name + "-tab";
            var href = TagHelper?.Href;
            var title = TagHelper?.Title;

            if (!string.IsNullOrEmpty(TagHelper?.ParentDropdownName))
            {
                return "<a class=\"dropdown-item\" id=\"" + id + "\" href=\"" + href + "\">" + title + "</a>";
            }

            return "<li class=\"nav-item\"><a class=\"nav-link" + TabItemActivePlaceholder + "\" id=\"" + id + "\" href=\"" + href + "\">" +
                   title +
                   "</a></li>";
        }

        protected virtual void SetPlaceholderForNameIfNotProvided()
        {
            if (TagHelper is not null && string.IsNullOrEmpty(TagHelper.Name))
            {
                TagHelper.Name = TabItemNamePlaceHolder;
            }
        }
    }
}

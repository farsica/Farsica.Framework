﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class TabTagHelperService : TagHelperService<TabTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();

            var innerContent = await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem(context, output);
            var tabContent = GetTabContentItem(context, output, innerContent.GetContent());

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);
            if (tabHeaderItems is not null)
            {
                var active = TagHelper?.Active ?? false;
                tabHeaderItems.Add(new TabItem(tabHeader, tabContent, active, TagHelper?.Name, TagHelper?.ParentDropdownName, false));
            }

            output.SuppressOutput();
        }

        protected virtual string? GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper?.Name + "-tab";
            var link = TagHelper?.Name;
            var control = TagHelper?.Name;
            var title = TagHelper?.Title;
            var attributes = GetTabHeaderAttributes(context, output);

            var classAttributesAsString = attributes.Where(a => a.Name == "class").ToList().Select(a => a.Value).JoinAsString(" ");
            var otherAttributesAsString = attributes.Where(a => a.Name != "class").ToList().ToHtmlAttributesAsString();

            if (!string.IsNullOrEmpty(TagHelper?.ParentDropdownName))
            {
                return "<a class=\"dropdown-item " + classAttributesAsString + "\" id=\"" + id + "\" href=\"#" + link + "\" data-toggle=\"tab\"  role=\"tab\" aria-controls=\"" + control + "\" aria-selected=\"false\" " + otherAttributesAsString + ">" + title + "</a>";
            }

            return "<li class=\"nav-item\"><a class=\"nav-link " + classAttributesAsString + " " + TabItemActivePlaceholder + "\" id=\"" + id + "\" data-toggle=\"" + TabItemsDataTogglePlaceHolder + "\" href=\"#" + link + "\" role=\"tab\" aria-controls=\"" + control + "\" aria-selected=\"" + TabItemSelectedPlaceholder + "\" " + otherAttributesAsString + ">" +
                   title +
                   "</a></li>";
        }

        protected virtual string? GetTabContentItem(TagHelperContext context, TagHelperOutput output, string? content)
        {
            var headerId = TagHelper?.Name + "-tab";
            var id = TagHelper?.Name;
            var attributes = GetTabContentAttributes(context, output);

            var classAttributesAsString = attributes.Where(a => a.Name == "class").ToList().Select(a => a.Name).JoinAsString(" ");
            var otherAttributesAsString = attributes.Where(a => a.Name != "class").ToList().ToHtmlAttributesAsString();

            return "<div class=\"tab-pane fade " + classAttributesAsString + " " + TabItemShowActivePlaceholder + "\" id=\"" + id + "\" role=\"tabpanel\" aria-labelledby=\"" + headerId + "\" " + otherAttributesAsString + ">" +
                   content +
                   "</div>";
        }

        protected virtual void SetPlaceholderForNameIfNotProvided()
        {
            if (TagHelper is not null && string.IsNullOrEmpty(TagHelper.Name))
            {
                TagHelper.Name = TabItemNamePlaceHolder;
            }
        }

        protected virtual IList<TagHelperAttribute> GetTabContentAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var contentprefix = "content-";
            return GetTabAttributesByPrefix(output.Attributes, contentprefix);
        }

        protected virtual IList<TagHelperAttribute> GetTabHeaderAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var headerprefix = "header-";
            return GetTabAttributesByPrefix(output.Attributes, headerprefix);
        }

        private static IList<TagHelperAttribute> GetTabAttributesByPrefix(TagHelperAttributeList attributes, string prefix)
        {
            return attributes.Where(a => a.Name.StartsWith(prefix))
                .Select(a => new TagHelperAttribute(a.Name[prefix.Length..], a.Value)).ToList();
        }
    }
}

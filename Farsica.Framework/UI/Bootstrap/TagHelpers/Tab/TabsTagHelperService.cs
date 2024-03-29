﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Grid;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class TabsTagHelperService : TagHelperService<TabsTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomNameIfNotProvided();

            var items = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            var headers = GetHeaders(context, output, items);
            var contents = GetConents(context, output, items);

            var surroundedHeaders = SurroundHeaders(context, output, headers);
            var surroundedContents = SurroundContents(context, output, contents);

            var finalContent = CombineHeadersAndContents(context, output, surroundedHeaders, surroundedContents);

            output.TagName = "div";
            output.Content.SetHtmlContent(finalContent);
            if (TagHelper?.TabStyle == TabStyle.PillVertical)
            {
                PlaceInsideRow(output);
            }
        }

        protected virtual string? CombineHeadersAndContents(TagHelperContext context, TagHelperOutput output, string? headers, string? contents)
        {
            var combined = new StringBuilder();

            if (TagHelper?.TabStyle == TabStyle.PillVertical)
            {
                var headerColumnSize = GetHeaderColumnSize();
                var contentColumnSize = 12 - headerColumnSize;

                headers = PlaceInsideColumn(headers, headerColumnSize);
                contents = PlaceInsideColumn(contents, contentColumnSize);
            }

            combined.AppendLine(headers).AppendLine(contents);

            return combined.ToString();
        }

        protected virtual string? SurroundHeaders(TagHelperContext context, TagHelperOutput output, string? headers)
        {
            var id = TagHelper?.Name;
            var navClass = TagHelper?.TabStyle == TabStyle.Tab ? " nav-tabs" : " nav-pills";
            var verticalClass = GetVerticalPillClassIfVertical();

            var surroundedHeaders = "<ul class=\"nav" + verticalClass + navClass + "\" id=\"" + id + "\" role=\"tablist\">" + Environment.NewLine +
                                   headers +
                                   "</ul>";

            return surroundedHeaders;
        }

        protected virtual string? SurroundContents(TagHelperContext context, TagHelperOutput output, string? contents)
        {
            var id = TagHelper?.Name + "Content";

            var surroundedContents = "<div class=\"tab-content\" id=\"" + id + "\">" + Environment.NewLine +
                                   contents +
                                   "   </div>";

            return surroundedContents;
        }

        protected virtual string? PlaceInsideColumn(string? contents, int columnSize)
        {
            return "<div class=\"col-md-" + columnSize + "\">" + Environment.NewLine + contents + "   </div>";
        }

        protected virtual void PlaceInsideRow(TagHelperOutput output)
        {
            output.Attributes.AddClass("row");
        }

        protected virtual void SetActiveTab(IReadOnlyList<TabItem> items)
        {
            if (!items.Exists(t => t.Active) && items.Count > 0)
            {
                var firstItem = items.FirstOrDefault(i => !i.IsDropdown);
                if (firstItem is not null)
                {
                    firstItem.Active = true;
                }
            }

            foreach (var tabItem in items)
            {
                if (tabItem is null)
                {
                    continue;
                }

                if (tabItem.Active)
                {
                    tabItem.Content = tabItem.Content?.Replace(TabItemShowActivePlaceholder, " show active");
                    tabItem.Header = tabItem.Header?.Replace(TabItemActivePlaceholder, " active").Replace(TabItemSelectedPlaceholder, "true");
                }
                else
                {
                    tabItem.Content = tabItem.Content?.Replace(TabItemShowActivePlaceholder, string.Empty);
                    tabItem.Header = tabItem.Header?.Replace(TabItemActivePlaceholder, string.Empty).Replace(TabItemSelectedPlaceholder, "false");
                }
            }
        }

#pragma warning disable CA1002 // Do not expose generic lists
        protected virtual string? GetHeaders(TagHelperContext context, TagHelperOutput output, List<TabItem> items)
#pragma warning restore CA1002 // Do not expose generic lists
        {
            SetActiveTab(items);

            var headersBuilder = new StringBuilder();

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var header = string.Empty;
                if (item.IsDropdown)
                {
                    var childHeaders = items.Where(i => i.ParentId == item.Id).Select(c => SetTabItemNameIfNotProvided(c.Header, items.IndexOf(c)));
                    var childHeadersAsString = string.Join(Environment.NewLine, childHeaders.ToArray());
                    header = item.Header?.Replace(TabDropdownItemsActivePlaceholder, childHeadersAsString);
                }
                else if (string.IsNullOrEmpty(item.ParentId))
                {
                    header = item.Header;

                    header = SetTabItemNameIfNotProvided(header, index);
                }

                headersBuilder.AppendLine(header);
            }

            var headers = SetDataToggle(headersBuilder.ToString());

            return headers;
        }

        protected virtual string? GetConents(TagHelperContext context, TagHelperOutput output, IReadOnlyList<TabItem> items)
        {
            var contentsBuilder = new StringBuilder();

            for (var index = 0; index < items.Count; index++)
            {
                if (items[index].IsDropdown)
                {
                    continue;
                }

                var content = items[index].Content;

                content = SetTabItemNameIfNotProvided(content, index);

                contentsBuilder.AppendLine(content);
            }

            return contentsBuilder.ToString();
        }

#pragma warning disable CA1002 // Do not expose generic lists
        protected virtual List<TabItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
#pragma warning restore CA1002 // Do not expose generic lists
        {
            var items = new List<TabItem>();
            context.Items[TabItems] = items;
            return items;
        }

        protected virtual string? GetDataToggleStyle()
        {
            return TagHelper?.TabStyle == TabStyle.Tab ? "tab" : "pill";
        }

        protected virtual string? SetDataToggle(string content)
        {
            return content.Replace(TabItemsDataTogglePlaceHolder, GetDataToggleStyle());
        }

        protected virtual string? GetVerticalPillClassIfVertical()
        {
            return TagHelper?.TabStyle == TabStyle.PillVertical ? " flex-column " : string.Empty;
        }

        protected virtual int GetHeaderColumnSize()
        {
            return TagHelper is null || TagHelper.VerticalHeaderSize is ColumnSize.Undefined or ColumnSize.Auto or ColumnSize._
                    ? (int)ColumnSize._3
                    : (int)TagHelper.VerticalHeaderSize;
        }

        protected virtual void SetRandomNameIfNotProvided()
        {
            if (TagHelper is not null && string.IsNullOrEmpty(TagHelper.Name))
            {
                TagHelper.Name = "T" + Guid.NewGuid().ToString("N");
            }
        }

        protected virtual string? SetTabItemNameIfNotProvided(string? content, int index)
        {
            return content?.Replace(TabItemNamePlaceHolder, TagHelper?.Name + "_" + index);
        }
    }
}

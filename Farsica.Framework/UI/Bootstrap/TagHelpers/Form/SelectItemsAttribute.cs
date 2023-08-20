namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItemsAttribute : Attribute
    {
        public SelectItemsAttribute(string itemsListPropertyName)
        {
            ItemsListPropertyName = itemsListPropertyName;
        }

        public string? ItemsListPropertyName { get; internal set; }

        public IEnumerable<SelectListItem>? GetItems(ModelExplorer explorer)
        {
            var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName?.Equals(ItemsListPropertyName) is true).ToList();

            while (properties.Count == 0)
            {
                explorer = explorer.Container;
                if (explorer.Container is null)
                {
                    return null;
                }

                properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName?.Equals(ItemsListPropertyName) is true).ToList();
            }

            return properties.First().Model as IEnumerable<SelectListItem>;
        }
    }
}

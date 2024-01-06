namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItemsAttribute(string itemsListPropertyName) : Attribute
    {
        public string? ItemsListPropertyName { get; internal set; } = itemsListPropertyName;

        public IEnumerable<SelectListItem>? GetItems(ModelExplorer explorer)
        {
            var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName?.Equals(ItemsListPropertyName) == true).ToList();

            while (properties.Count == 0)
            {
                explorer = explorer.Container;
                if (explorer.Container is null)
                {
                    return null;
                }

                properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName?.Equals(ItemsListPropertyName) == true).ToList();
            }

            return properties.First().Model as IEnumerable<SelectListItem>;
        }
    }
}

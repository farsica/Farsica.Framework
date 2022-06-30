namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public static class TagHelperAttributeExtensions
    {
        public static string ToHtmlAttributeAsString(this TagHelperAttribute attribute)
        {
            return attribute.Name + "=\"" + attribute.Value + "\"";
        }

        public static string ToHtmlAttributesAsString(this List<TagHelperAttribute> attributes)
        {
            StringBuilder sb = new();
            for (int i = 0; i < attributes.Count; i++)
            {
                sb.Append(attributes[i].ToHtmlAttributeAsString() + " ");
            }

            return sb.ToString();
        }
    }
}

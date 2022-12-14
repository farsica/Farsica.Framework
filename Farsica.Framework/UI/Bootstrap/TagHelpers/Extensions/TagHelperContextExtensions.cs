namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public static class TagHelperContextExtensions
    {
        public static T? GetValue<T>(this TagHelperContext context, string? key)
        {
            if (!context.Items.ContainsKey(key))
            {
                return default;
            }

            return (T)context.Items[key];
        }
    }
}

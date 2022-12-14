namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions
{
    using System.IO;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public static class TagHelperOutputExtensions
    {
        public static string? Render(this TagHelperOutput output, HtmlEncoder htmlEncoder)
        {
            using var writer = new StringWriter();
            output.WriteTo(writer, htmlEncoder);
            return writer.ToString();
        }
    }
}

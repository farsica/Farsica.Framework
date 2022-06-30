namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Farsica.Framework.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [Injectable]
    public class GridViewColumnsTagHelperService : TagHelperService<GridViewColumnsTagHelper>
    {
        private readonly HtmlEncoder htmlEncoder;
        private readonly IHtmlGenerator htmlGenerator;
        private readonly IServiceProvider serviceProvider;

        public GridViewColumnsTagHelperService(
            HtmlEncoder htmlEncoder,
            IHtmlGenerator htmlGenerator,
            IServiceProvider serviceProvider)
        {
            this.htmlEncoder = htmlEncoder;
            this.htmlGenerator = htmlGenerator;
            this.serviceProvider = serviceProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();
            output.SuppressOutput();
        }
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Farsica.Framework.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [Injectable]
    public class GridViewTagHelperService : TagHelperService<GridViewTagHelper>
    {
        private readonly HtmlEncoder htmlEncoder;
        private readonly IHtmlGenerator htmlGenerator;
        private readonly IServiceProvider serviceProvider;

        public GridViewTagHelperService(
            HtmlEncoder htmlEncoder,
            IHtmlGenerator htmlGenerator,
            IServiceProvider serviceProvider)
        {
            this.htmlEncoder = htmlEncoder;
            this.htmlGenerator = htmlGenerator;
            this.serviceProvider = serviceProvider;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            NormalizeTagMode(context, output);

            // await ProcessFieldsAsync(context, output);
        }

        protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
        }

        protected virtual IReadOnlyList<ModelExpression>? GetModel(TagHelperContext context, TagHelperOutput output)
        {
            var type = TagHelper.For.ModelExplorer.ModelType;
            if (typeof(IGridView<,>) != type)
            {
                throw new NotSupportedException("Model must implement IGridView<TModel,Tsearch>");
            }

            return null;

            // type.GetProperties().Where(t=>t.)
            // return TagHelper.For.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively);
        }
    }
}

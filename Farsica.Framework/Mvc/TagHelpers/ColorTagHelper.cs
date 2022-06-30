namespace Farsica.Framework.Mvc.TagHelpers
{
    using System.Text.Encodings.Web;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-color", TagStructure = TagStructure.WithoutEndTag)]
    public class ColorTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;
        private readonly HtmlEncoder encoder;

        public ColorTagHelper(IHtmlGenerator generator, HtmlEncoder encoder, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
            this.encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("input-group");
            output.Attributes.Add("data-toggle", "colorpicker");
            output.TagMode = TagMode.StartTagAndEndTag;

            var attributeList = new TagHelperAttributeList { { "name", ElementName }, { "type", "text" }, { "dir", "ltr" } };
            attributeList.AddClass("form-control input-lg");

            var val = Value?.ToString() ?? For.Model?.ToString();
            if (!val.IsNullOrEmpty())
            {
                attributeList.Add("value", val);
            }

            string innerHtml = null;
            using (var writer = new System.IO.StringWriter())
            {
                var tagBuilder = generator.GenerateTextBox(ViewContext, For.ModelExplorer, ElementName, val, null, attributeList);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                innerHtml = writer.ToString();
            }

            var postContent = @"
                <span class='input-group-append'>
                    <span class='input-group-text colorpicker-input-addon'><i></i></span>
                </span>";
            output.Content.SetHtmlContent(innerHtml + postContent);
        }
    }
}

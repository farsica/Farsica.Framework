namespace Farsica.Framework.Mvc.TagHelpers
{
    using System.Text;
    using System.Text.Encodings.Web;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-editor", TagStructure = TagStructure.WithoutEndTag)]
    public class HtmlEditorTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;
        private readonly HtmlEncoder encoder;

        public HtmlEditorTagHelper(IHtmlGenerator generator, HtmlEncoder encoder, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
            this.encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            StringBuilder sb = new();
            sb.Append("<div class=\"editor-wrapper\">");

            output.Attributes.AddIfNotExist("name", ElementName);
            output.Attributes.AddIfNotExist("rows", "6");
            output.Attributes.AddIfNotExist("dir", Globals.IsRtl ? "rtl" : "ltr");
            output.Attributes.AddClass("textarea");

            var val = Value?.ToString() ?? For.Model?.ToString();
            if (!val.IsNullOrEmpty())
            {
                output.Attributes.AddIfNotExist("value", val);
            }

            using (var writer = new System.IO.StringWriter())
            {
                var tagBuilder = generator.GenerateTextArea(ViewContext, For.ModelExplorer, ElementName, 6, 0, output.Attributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            sb.Append("</div>");
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
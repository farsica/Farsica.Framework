namespace Farsica.Framework.Mvc.TagHelpers
{
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.Mvc.ViewFeatures;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-file", TagStructure = TagStructure.WithoutEndTag)]
    public class FileTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;

        public FileTagHelper(IHtmlGenerator generator, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
        }

        [HtmlAttributeName("frb-old-for")]
        public ModelExpression OldFor { get; set; }

        [HtmlAttributeName("frb-old-name")]
        public string? OldName { get; set; }

        [HtmlAttributeName("frb-old-value")]
        public string? OldValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            StringBuilder sb = new();
            sb.Append("<div class=\"uploader\">");
            sb.Append("<div class='custom-file'>");

            output.Attributes.AddIfNotExist("name", ElementName);
            output.Attributes.AddIfNotExist("type", "file");
            output.Attributes.AddIfNotExist("data-toggle", "file");
            output.Attributes.AddClass("float-left custom-file-input");

            var val = Value?.ToString() ?? For.Model?.ToString();
            if (!val.IsNullOrEmpty())
            {
                output.Attributes.Add("value", val);
            }

            using (var writer = new System.IO.StringWriter())
            {
                var tagBuilder = generator.GenerateTextBox(ViewContext, For.ModelExplorer, ElementName, val, null, output.Attributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            var oldVal = OldValue?.ToString() ?? OldFor?.Model?.ToString();
            sb.Append($"<label class='custom-file-label' for='{ElementId}'>{(Globals.IsImage(oldVal) ? $"<img class='h-100' src='{oldVal}' alt='{For.Name}' />" : oldVal?.Split('/').LastOrDefault())}</label>");
            sb.Append("</div>");

            sb.Append($"<a class='btn btn-light border float-right text-green {(oldVal.IsNullOrEmpty() ? "disabled" : string.Empty)}' {(Globals.IsImage(oldVal) ? "download='image.png'" : string.Empty)} href='{oldVal}'><i class='fas fa-download'></i></a>");
            sb.Append($"<button class='btn btn-light border float-right margin-r-5 text-red' {(oldVal.IsNullOrEmpty() ? "disabled='disabled" : "'")} type='button'><i class='fas fa-times'></i></button>");

            using (var writer = new System.IO.StringWriter())
            {
                var elementOldName = NameAndIdProvider.GetFullHtmlFieldName(ViewContext, OldName ?? OldFor.Name);
                var hiddenAttributes = new TagHelperAttributeList { { "type", "hidden" }, { "name", elementOldName } };
                if (!oldVal.IsNullOrEmpty())
                {
                    hiddenAttributes.Add("value", oldVal);
                }

                var tagBuilder = generator.GenerateHidden(ViewContext, OldFor?.ModelExplorer, elementOldName, oldVal, false, hiddenAttributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            sb.Append("<div class='clearfix'></div>");

            sb.Append("</div>");
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}

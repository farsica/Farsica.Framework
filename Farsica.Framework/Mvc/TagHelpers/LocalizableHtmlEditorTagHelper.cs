namespace Farsica.Framework.Mvc.TagHelpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.Data;
    using Farsica.Framework.Resources;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-localizable-editor", TagStructure = TagStructure.WithoutEndTag)]
    public class LocalizableHtmlEditorTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;

        public LocalizableHtmlEditorTagHelper(IHtmlGenerator generator, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
        }

        [HtmlAttributeName("frb-languages")]
        public IEnumerable<Language> Languages { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            StringBuilder sb = new();
            sb.Append("<div class=\"input-prepend input-append localize-wrapper\">");
            sb.Append("<div class='localize editor-wrapper'>");

            output.Attributes.AddIfNotExist("name", ElementName);
            output.Attributes.AddIfNotExist("rows", "6");
            output.Attributes.AddIfNotExist("dir", Globals.IsRtl ? "rtl" : "ltr");
            output.Attributes.AddClass("textarea");

            var elementValue = Value as LocalizableString ?? For.Model as LocalizableString;
            if (elementValue is not null && !elementValue.Value.IsNullOrEmpty())
            {
                output.Attributes.AddIfNotExist("value", elementValue.Value);
            }

            using (var writer = new System.IO.StringWriter())
            {
                var tagBuilder = generator.GenerateTextArea(ViewContext, For.ModelExplorer, ElementName, 6, 0, output.Attributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            sb.Append("<div class='dropdown'>");
            sb.Append("<button class='btn btn-light border dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='false'><i class='fas fa-language'></i></button>");
            sb.Append("<ul class='dropdown-menu abc-checkbox abc-checkbox-success'>");
            sb.Append("<li class='dropdown-item'>&nbsp;<input type='checkbox' checked='checked' disabled='disabled'><i>&nbsp;</i><label>" + GlobalResource.InvariantCulture + "</label></li>");

            StringBuilder innerSb = new();
            int i = 0;
            foreach (var dto in Languages)
            {
                var data = elementValue?.LocalizedValues.FirstOrDefault(t => t.Id == dto.Id);
                var id = "culture" + dto.Id;
                var checkedAttr = data is not null ? "checked='checked'" : string.Empty;
                sb.Append("<li class='dropdown-item'>&nbsp;<input id='" + id + "' type='checkbox' " + (data is not null ? "checked='checked'" : string.Empty) + " /><i>&nbsp;</i><label for='" + id + "'>" + dto.Name + "</label></li>");

                var css = $"localize wrapper-{dto.Id} {(data is not null ? string.Empty : "collapse")}";
                innerSb.Append("<div class='editor-wrapper mt-3 " + css + "'>");

                using (var writer = new System.IO.StringWriter())
                {
                    var innerName = $"{ElementName}.{nameof(LocalizableString.LocalizedValues)}[{i}].Value";
                    var innerAttributes = new TagHelperAttributeList { { "data-culture", dto.Id }, { "name", innerName }, { "value", data?.Value } };
                    innerAttributes.Merge(output.Attributes);
                    var tagBuilder = generator.GenerateTextArea(ViewContext, For.ModelExplorer, innerName, 6, 0, innerAttributes);
                    tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                    innerSb.Append(writer.ToString());
                }

                innerSb.Append("<span class='badge badge-info'>" + dto.Name + "</span>");
                innerSb.Append($"<input type='hidden' name='{ElementName}.{nameof(LocalizableString.LocalizedValues)}[{i}].Id' value='{data?.Id}'/>");

                innerSb.Append("</div>");

                i++;
            }

            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(innerSb);
            sb.Append("</div>");
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}

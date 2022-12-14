namespace Farsica.Framework.Mvc.TagHelpers
{
    using System;
    using System.Text;
    using System.Text.Encodings.Web;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    public enum PickerType
    {
        Date,
        DateTime,
        TimeSpan,
        Month,
    }

    [HtmlTargetElement("frb-date-time", TagStructure = TagStructure.WithoutEndTag)]
    public class DateTimeTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;
        private readonly HtmlEncoder encoder;

        public DateTimeTagHelper(IHtmlGenerator generator, HtmlEncoder encoder, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
            this.encoder = encoder;
        }

        [HtmlAttributeName("frb-type")]
        public PickerType PickerType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            StringBuilder sb = new();
            sb.Append("<div class=\"input-group\">");

            var toggle = "datepicker";
            var format = "d";
            switch (PickerType)
            {
                case PickerType.DateTime:
                    if (For.Metadata.ModelType != typeof(DateTime) && For.Metadata.ModelType != typeof(DateTime?) && For.Metadata.ModelType != typeof(DateTimeOffset) && For.Metadata.ModelType != typeof(DateTimeOffset?))
                    {
                        throw new ArgumentException(nameof(For.Metadata.ModelType));
                    }

                    toggle = "datetimepicker";
                    format = "yyyy/MM/dd HH:mm";
                    break;
                case PickerType.TimeSpan:
                    if (For.Metadata.ModelType != typeof(TimeSpan) && For.Metadata.ModelType != typeof(TimeSpan?))
                    {
                        throw new ArgumentException(nameof(For.Metadata.ModelType));
                    }

                    toggle = "timepicker";
                    format = "hh\\:mm";
                    break;
                case PickerType.Month:
                    if (For.Metadata.ModelType != typeof(DateTime) && For.Metadata.ModelType != typeof(DateTime?) && For.Metadata.ModelType != typeof(DateTimeOffset) && For.Metadata.ModelType != typeof(DateTimeOffset?))
                    {
                        throw new ArgumentException(nameof(For.Metadata.ModelType));
                    }

                    toggle = "monthpicker";
                    format = "yyyy/MM";
                    break;
            }

            output.Attributes.AddIfNotExist("name", ElementName);
            output.Attributes.AddIfNotExist("type", "text");
            output.Attributes.AddIfNotExist("autocomplete", "off");
            output.Attributes.AddIfNotExist("data-toggle", toggle);
            output.Attributes.AddClass("form-control");

            string? val = null;
            if (Value != null)
            {
                val = (Value as TimeSpan?)?.ToString(format) ?? (Value as DateTimeOffset?)?.ToString(format) ?? (Value as DateTime?)?.ToString(format);
            }
            else if (For.Model != null)
            {
                val = (For.Model as TimeSpan?)?.ToString(format) ?? (For.Model as DateTimeOffset?)?.ToString(format) ?? (For.Model as DateTime?)?.ToString(format);
            }

            if (!val.IsNullOrEmpty())
            {
                output.Attributes.AddIfNotExist("value", val);
            }

            using (var writer = new System.IO.StringWriter())
            {
                var tagBuilder = generator.GenerateTextBox(ViewContext, For.ModelExplorer, ElementName, val, null, output.Attributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            sb.Append("<span class=\"input-group-append\">");
            sb.Append("<button class=\"btn btn-light border\" type=\"button\" onclick=\"$('#" + ElementId + "').val('');\"><i class=\"fas fa-times\"></i></button>");
            sb.Append("</span>");
            sb.Append("<input type='hidden' id='" + ElementId + "_dp' name='" + ElementName + ".dp' value='dp' />");

            sb.Append("</div>");
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}

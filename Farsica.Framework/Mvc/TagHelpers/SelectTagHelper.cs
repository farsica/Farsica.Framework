namespace Farsica.Framework.Mvc.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.UI.Bootstrap.TagHelpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-select", TagStructure = TagStructure.WithoutEndTag)]
    public class SelectTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        private readonly IHtmlGenerator generator;

        public SelectTagHelper(IHtmlGenerator generator, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            this.generator = generator;
        }

        [HtmlAttributeName("frb-multiple")]
        public bool Multiple { get; set; } = false;

        [HtmlAttributeName("frb-tags")]
        public bool Tags { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (For.Metadata.IsEnum is false && For.Metadata.ElementType is null)
            {
                throw new ArgumentException("ModelType must be an Enum or implements System.Collections.IEnumerable");
            }

            var isEnum = For.Metadata.IsEnum || For.Metadata.ElementMetadata.IsEnum;
            var isPrimitive = isEnum is false && For.Metadata.ElementType.IsSimpleType();
            var placeholder = For.Metadata.Placeholder ?? Resources.GlobalResource.Select;
            var isFlagsEnum = isEnum && (For.Metadata.IsFlagsEnum is true || For.Metadata.ElementMetadata?.IsFlagsEnum is true);

            IEnumerable<SelectListItem>? items = null;
            IEnumerable<string>? currentValues = null;
            if (isEnum)
            {
                IEnumerable<Enum> values;
                if (Value is IEnumerable<Enum> @enum1)
                {
                    values = @enum1;
                    Multiple = true;
                }
                else if (Value is Enum @enum2)
                {
                    values = new List<Enum>(1) { @enum2 };
                }
                else if (For.Metadata.ElementType is not null)
                {
                    values = For.Model as IEnumerable<Enum>;
                    Multiple = true;
                }
                else
                {
                    values = new List<Enum>(1) { For.Model.As<Enum>() };
                }

                (items, currentValues) = GenerateEnumItems(values, isFlagsEnum);
            }
            else if (isPrimitive)
            {
                IEnumerable<object> values;
                if (Value is IEnumerable<object> @primitive1)
                {
                    values = @primitive1;
                    Multiple = true;
                }
                else if (Value is not null)
                {
                    values = new List<object>(1) { Value };
                }
                else if (For.Model is IEnumerable<object> @primitive2)
                {
                    values = @primitive2;
                    Multiple = true;
                }
                else
                {
                    values = new List<object>(1) { For.Model };
                }

                items = values?.Select(t => new SelectListItem
                {
                    Value = t.ToString(),
                    Text = t.ToString(),
                    Selected = true,
                });
                currentValues = items?.Select(t => t.Value);
            }
            else
            {
                if (Value is IEnumerable<SelectListItem> listItem1)
                {
                    items = listItem1;
                }
                else if (For.Model is IEnumerable<SelectListItem> listItem2)
                {
                    items = listItem2;
                }

                currentValues = items?.Where(t => t.Selected)?.Select(t => t.Value);
            }

            StringBuilder sb = new();

            if (isEnum && Multiple)
            {
                sb.Append($"<div class=\"select-wrapper\" data-name=\"{ElementName}\" data-name-list=\"{ElementName}\" data-post-fix-value=\"\" data-list=\"false\" data-flags=\"{(isFlagsEnum ? "true" : "false")}\">");
                if (currentValues is not null)
                {
                    var i = 0;
                    foreach (var item in currentValues)
                    {
                        sb.Append($"<input type=\"hidden\" name=\"{ElementName}[{i}]\" value=\"{item:d}\" />");
                        i++;
                    }
                }

                if (isFlagsEnum && Convert.ToInt64(For.Model) > 0)
                {
                    sb.Append($"<input type=\"hidden\" name=\"{ElementName}\" value=\"{(For.Model as Enum)?.ToString("d")}\" />");
                }

                sb.Append("</div>");
            }
            else if (isPrimitive)
            {
                sb.Append($"<div class=\"select-wrapper\" data-name=\"{ElementName}\" data-name-list=\"{ElementName}\" data-post-fix-value=\"\" data-list=\"true\">");
                if (currentValues is not null)
                {
                    int i = 0;
                    foreach (var item in currentValues)
                    {
                        sb.Append($"<input type=\"hidden\" name=\"{ElementName}[{i}]\" value=\"{item}\" />");

                        i++;
                    }
                }

                sb.Append("</div>");
            }
            else
            {
                sb.Append($"<div class=\"select-wrapper\" data-name=\"{ElementName}\" data-name-list=\"{ElementName}\" data-post-fix-value=\".Value\" data-post-fix-selected=\".Selected\" data-list=\"true\">");

                if (currentValues is not null)
                {
                    int i = 0;
                    foreach (var item in currentValues)
                    {
                        sb.Append($"<input type=\"hidden\" name=\"{ElementName}[{i}].Value\" value=\"{item}\" />");
                        sb.Append($"<input type=\"hidden\" name=\"{ElementName}[{i}].Selected\" value=\"true\" />");

                        i++;
                    }
                }

                sb.Append("</div>");
            }

            using (var writer = new System.IO.StringWriter())
            {
                var name = ElementName + (isEnum is false || Multiple ? ".s" : string.Empty);
                ViewContext.ViewData.Add(name, Multiple ? string.Join(",", currentValues) : currentValues?.FirstOrDefault());
                output.Attributes.AddIfNotExist("name", name);
                output.Attributes.AddIfNotExist("data-toggle", "select");
                output.Attributes.AddIfNotExist("data-placeholder", placeholder);
                if (Tags)
                {
                    output.Attributes.AddIfNotExist("data-tags", "true");
                    output.Attributes.AddIfNotExist("data-allow-clear", "false");
                    output.Attributes.AddClass("full-width");
                }

                if (items.Any(t => t.Group is not null))
                {
                    output.Attributes.AddIfNotExist("data-group", "true");
                }

                if (Multiple)
                {
                    output.Attributes.AddIfNotExist("multiple", "multiple");
                }
                else
                {
                    items = new[] { new SelectListItem(placeholder, string.Empty) }.Concat(items);
                }

                var tagBuilder = generator.GenerateSelect(ViewContext, For?.ModelExplorer, null, name, items, currentValues.ToList(), Multiple, output.Attributes);
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }

            output.Content.SetHtmlContent(sb.ToString());
        }

        private (IEnumerable<SelectListItem> Items, IEnumerable<string> CurrentValues) GenerateEnumItems(IEnumerable<Enum> values, bool flags)
        {
            var type = For.Metadata.ElementMetadata?.UnderlyingOrModelType ?? For.Metadata.UnderlyingOrModelType;
            var ignoreFields = type.GetFields().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) is not null || t.GetCustomAttribute<DisplayAttribute>(false)?.Hidden is true).Select(t => (t.GetValue(null) as Enum)?.ToString("d"));

            var groupedDisplayNamesAndValues = For.Metadata.ElementMetadata?.EnumGroupedDisplayNamesAndValues ?? For.Metadata.EnumGroupedDisplayNamesAndValues;
            var lst = from t in groupedDisplayNamesAndValues
                      where !ignoreFields.Contains(t.Value)
                      select new SelectListItem
                      {
                          Group = t.Key.Group.IsNullOrEmpty() ? null : new SelectListGroup { Name = t.Key.Group },
                          Text = t.Key.Name,
                          Value = t.Value,
#pragma warning disable CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
                          Selected = values is not null && (flags ? values.Any(v => v?.HasFlag(Enum.Parse(type, t.Value) as Enum) is true) : values.Any(v => v?.ToString("d") == t.Value)),
#pragma warning restore CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
                      };
            return (lst, lst?.Where(t => t.Selected).Select(t => t?.Value));
        }
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [Injectable]
    public class InputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder) : TagHelperService<InputTagHelper>
    {
        private readonly IHtmlGenerator generator = generator;
        private readonly HtmlEncoder encoder = encoder;
        private IEnumerable<Attribute>? cachedModelAttributes;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            cachedModelAttributes = TagHelper.For.ModelExplorer.GetAttributes();

            var (innerHtml, isCheckBox) = await GetFormInputGroupAsHtmlAsync(context, output);

            var order = TagHelper.For.ModelExplorer.GetDisplayOrder();

            AddGroupToFormGroupContents(
                context,
                TagHelper.For.Name,
                SurroundInnerHtmlAndGet(context, output, innerHtml, isCheckBox),
                order,
                out var suppress);

            if (suppress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = "div";
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass(isCheckBox ? "custom-checkbox" : "form-group");
                output.Attributes.AddClass(isCheckBox ? "custom-control" : string.Empty);
                output.Attributes.AddClass(isCheckBox ? "mb-2" : string.Empty);
                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
            }
        }

        protected virtual async Task<(string? Content, bool IsCheckBox)> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
        {
            var (inputTag, isCheckBox) = await GetInputTagHelperOutputAsync(context, output);

            var inputHtml = inputTag.Render(encoder);
            var label = await GetLabelAsHtmlAsync(context, output, inputTag, isCheckBox);
            var info = GetInfoAsHtml(context, output, inputTag, isCheckBox);
            var validation = isCheckBox ? string.Empty : await GetValidationAsHtmlAsync(context, output, inputTag);

            return (GetContent(context, output, label, inputHtml, validation, info, isCheckBox), isCheckBox);
        }

        protected virtual async Task<string?> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            if (IsOutputHidden(inputTag))
            {
                return string.Empty;
            }

            var validationMessageTagHelper = new ValidationMessageTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext,
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return await validationMessageTagHelper.RenderAsync(attributeList, context, encoder, "span", TagMode.StartTagAndEndTag);
        }

        protected virtual string? GetContent(TagHelperContext context, TagHelperOutput output, string? label, string? inputHtml, string? validation, string? infoHtml, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + label :
                label + inputHtml;

            return innerContent + infoHtml + validation;
        }

        protected virtual string? SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string? innerHtml, bool isCheckbox)
        {
            return "<div class=\"" + (isCheckbox ? "custom-checkbox custom-control" : "form-group") + "\">" +
                   Environment.NewLine + innerHtml + Environment.NewLine +
                   "</div>";
        }

        protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            if (dataTypeAttribute?.ElementDataType == ElementDataType.MultilineText)
            {
                var textAreaTagHelper = new TextAreaTagHelper(generator)
                {
                    For = TagHelper.For,
                    ViewContext = TagHelper.ViewContext,
                };

                if (!string.IsNullOrEmpty(TagHelper.Name))
                {
                    textAreaTagHelper.Name = TagHelper.Name;
                }

                return textAreaTagHelper;
            }

            var inputTagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper(generator)
            {
                For = TagHelper.For,
                InputTypeName = TagHelper.InputTypeName,
                ViewContext = TagHelper.ViewContext,
            };

            if (!TagHelper.Format.IsNullOrEmpty())
            {
                inputTagHelper.Format = TagHelper.Format;
            }

            if (!TagHelper.Name.IsNullOrEmpty())
            {
                inputTagHelper.Name = TagHelper.Name;
            }

            if (!(TagHelper.Value as string).IsNullOrEmpty())
            {
                inputTagHelper.Value = TagHelper.Value as string;
            }

            return inputTagHelper;
        }

        protected virtual async Task<(TagHelperOutput InputTagHelperOutput, bool IsCheckbox)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagHelper = GetInputTagHelper(context, output);

            var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
                GetInputAttributes(context, output),
                context,
                "input");

            ConvertToTextAreaIfTextArea(inputTagHelperOutput);
            AddDisabledAttribute(inputTagHelperOutput);
            AddAutoFocusAttribute(inputTagHelperOutput);
            var isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
            AddFormControlClass(context, output, isCheckbox, inputTagHelperOutput);
            AddReadOnlyAttribute(inputTagHelperOutput);
            AddPlaceholderAttribute(inputTagHelperOutput);
            AddInfoTextId(inputTagHelperOutput);

            return (inputTagHelperOutput, isCheckbox);
        }

        protected virtual void AddAutoFocusAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (TagHelper.AutoFocus && !inputTagHelperOutput.Attributes.ContainsName("data-auto-focus"))
            {
                inputTagHelperOutput.Attributes.Add("data-auto-focus", "true");
            }
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (!inputTagHelperOutput.Attributes.ContainsName("disabled") &&
                     (TagHelper.IsDisabled || cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Disabled == true))
            {
                inputTagHelperOutput.Attributes.Add("disabled", string.Empty);
            }
        }

        protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (!inputTagHelperOutput.Attributes.ContainsName("readonly") &&
                    (TagHelper.IsReadonly.GetValueOrDefault() || cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Readonly == true))
            {
                inputTagHelperOutput.Attributes.Add("readonly", string.Empty);
            }
        }

        protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
            {
                return;
            }

            var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
            if (attribute is not null)
            {
                var placeholderLocalized = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Prompt);
                if (!string.IsNullOrEmpty(placeholderLocalized))
                {
                    inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
                }
            }
        }

        protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
        {
            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");
            if (idAttr is null)
            {
                return;
            }

            var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
            if (attribute is not null)
            {
                var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
                if (!string.IsNullOrEmpty(description))
                {
                    inputTagHelperOutput.Attributes.Add("aria-describedby", description);
                }
            }
        }

        protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
        {
            return attributes.Any(t => t.Value is not null && t.Name == "type" && t.Value.ToString() == "checkbox");
        }

        protected virtual async Task<string?> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag) || TagHelper.SuppressLabel)
            {
                return string.Empty;
            }

            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (uIHintAttribute is not null && uIHintAttribute.LabelPosition == LabelPosition.Hidden)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(TagHelper.Label))
            {
                return await GetLabelAsHtmlUsingTagHelperAsync(context, output, isCheckbox) + GetRequiredSymbol(context, output);
            }

            var checkboxClass = isCheckbox ? "class=\"custom-control-label\" " : string.Empty;

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + TagHelper.Label +
                   "</label>" + GetRequiredSymbol(context, output);
        }

        protected virtual string? GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplayRequiredSymbol)
            {
                return string.Empty;
            }

            return cachedModelAttributes?.GetAttribute<RequiredAttribute>() is not null ? "<span> * </span>" : string.Empty;
        }

        protected virtual string? GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag))
            {
                return string.Empty;
            }

            if (isCheckbox)
            {
                return string.Empty;
            }

            var text = string.Empty;
            if (!string.IsNullOrEmpty(TagHelper.InfoText))
            {
                text = TagHelper.InfoText;
            }
            else
            {
                var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
                if (attribute is not null)
                {
                    var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
                    if (!string.IsNullOrEmpty(description))
                    {
                        text = description;
                    }
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
            return $"<small id=\"{idAttr?.Value}InfoText\" class=\"form-text text-muted\">{text}</small>";
        }

        protected virtual async Task<string?> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output, bool isCheckbox)
        {
            var labelTagHelper = new LabelTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext,
            };

            var attributeList = new TagHelperAttributeList();

            if (isCheckbox)
            {
                attributeList.AddClass("custom-control-label");
            }

            return await labelTagHelper.RenderAsync(attributeList, context, encoder, "label", TagMode.StartTagAndEndTag);
        }

        protected virtual void ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            if (dataTypeAttribute is null || dataTypeAttribute.ElementDataType != ElementDataType.MultilineText)
            {
                return;
            }

            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            tagHelperOutput.TagName = "textarea";
            tagHelperOutput.TagMode = TagMode.StartTagAndEndTag;
            tagHelperOutput.Content.SetContent(TagHelper.For.ModelExplorer.Model?.ToString());
            if (uIHintAttribute?.Rows > 0)
            {
                tagHelperOutput.Attributes.Add("rows", uIHintAttribute.Rows);
            }

            if (uIHintAttribute?.Cols > 0)
            {
                tagHelperOutput.Attributes.Add("cols", uIHintAttribute.Cols);
            }
        }

        protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";

            var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix)).ToList();

            var attrList = new TagHelperAttributeList();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                attrList.Add(tagHelperAttribute);
            }

            if (!TagHelper.InputTypeName.IsNullOrEmpty() && !attrList.ContainsName("type"))
            {
                attrList.Add("type", TagHelper.InputTypeName);
            }

            if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("name"))
            {
                attrList.Add("name", TagHelper.Name);
            }

            if (!(TagHelper.Value as string).IsNullOrEmpty() && !attrList.ContainsName("value"))
            {
                attrList.Add("value", TagHelper.Value);
            }

            return attrList;
        }

        protected virtual void LeaveOnlyGroupAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";
            var tagHelperAttributes = output.Attributes.Where(a => a.Name.StartsWith(groupPrefix)).ToList();

            output.Attributes.Clear();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                var nameWithoutPrefix = tagHelperAttribute.Name[groupPrefix.Length..];
                var newAttritube = new TagHelperAttribute(nameWithoutPrefix, tagHelperAttribute.Value);
                output.Attributes.Add(newAttritube);
            }
        }

        protected virtual string? GetSize(TagHelperContext context, TagHelperOutput output)
        {
            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (uIHintAttribute is not null)
            {
                TagHelper.Size = uIHintAttribute.Size;
            }

            switch (TagHelper.Size)
            {
                case FormControlSize.Small:
                    return "custom-select-sm";
                case FormControlSize.Medium:
                    return "custom-select-md";
                case FormControlSize.Large:
                    return "custom-select-lg";
                default:
                    return string.Empty;
            }
        }

        protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            return inputTag.Attributes.Any(t => t.Name.ToLowerInvariant() == "type" && t.Value.ToString()?.ToLowerInvariant() == "hidden")
                || dataTypeAttribute?.ElementDataType == ElementDataType.Hidden;
        }

        protected virtual string? GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr is not null ? "for=\"" + idAttr.Value + "\"" : string.Empty;
        }

        protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string? propertyName, string? html, int order, out bool suppress)
        {
            var list = context.GetValue<List<FormGroupItem>>(FormGroupContents) ?? [];
            suppress = list is null;

            if (list is not null && propertyName is not null && !list.Exists(igc => igc.HtmlContent?.Contains("id=\"" + propertyName.Replace('.', '_') + "\"") == true))
            {
                list.Add(new FormGroupItem
                {
                    HtmlContent = html,
                    Order = order,
                    PropertyName = propertyName,
                });
            }
        }

        private void AddFormControlClass(TagHelperContext context, TagHelperOutput output, bool isCheckbox, TagHelperOutput inputTagHelperOutput)
        {
            var className = "form-control";

            if (isCheckbox)
            {
                className = "custom-control-input";
            }

            inputTagHelperOutput.Attributes.AddClass(className + " " + GetSize(context, output));
        }
    }
}

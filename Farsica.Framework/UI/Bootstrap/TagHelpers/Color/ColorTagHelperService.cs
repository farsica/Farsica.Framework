// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.Encodings.Web;
// using System.Threading.Tasks;

// using Farsica.Framework.Core;
// using Farsica.Framework.Core.Extensions;
// using Farsica.Framework.DataAnnotation;
// using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
// using Farsica.Framework.UI.Bootstrap.TagHelpers.Form;

// using Microsoft.AspNetCore.Mvc.TagHelpers;
// using Microsoft.AspNetCore.Mvc.ViewFeatures;
// using Microsoft.AspNetCore.Razor.TagHelpers;

// namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Color
// {
//    [Injectable]
//    public class ColorTagHelperService : TagHelpers.TagHelperService<ColorTagHelper>
//    {
//        private readonly IHtmlGenerator generator;
//        private readonly HtmlEncoder encoder;
//        private IEnumerable<Attribute> cachedModelAttributes;

// public ColorTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
//        {
//            this.generator = generator;
//            this.encoder = encoder;
//        }

// public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            cachedModelAttributes = TagHelper.For.ModelExplorer.GetAttributes();

// var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output);

// var order = TagHelper.For.ModelExplorer.GetDisplayOrder();

// AddGroupToFormGroupContents(
//                context,
//                TagHelper.For.Name,
//                SurroundInnerHtmlAndGet(context, output, innerHtml),
//                order,
//                out var suppress
//            );

// if (suppress)
//            {
//                output.SuppressOutput();
//            }
//            else
//            {
//                output.TagMode = TagMode.StartTagAndEndTag;
//                output.TagName = "div";
//                LeaveOnlyGroupAttributes(context, output);
//                output.Attributes.AddClass("form-group");
//                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
//            }
//        }

// protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            var inputTag = await GetInputTagHelperOutputAsync(context, output);

// var inputHtml = inputTag.Render(encoder);
//            var label = await GetLabelAsHtmlAsync(context, output, inputTag);
//            var info = GetInfoAsHtml(context, output, inputTag);
//            var validation = await GetValidationAsHtmlAsync(context, output, inputTag);

// return GetContent(context, output, label, inputHtml, validation, info);
//        }

// protected virtual async Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
//        {
//            var validationMessageTagHelper = new ValidationMessageTagHelper(generator)
//            {
//                For = TagHelper.For,
//                ViewContext = TagHelper.ViewContext
//            };

// var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

// return await validationMessageTagHelper.RenderAsync(attributeList, context, encoder, "span", TagMode.StartTagAndEndTag);
//        }

// protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml, string validation, string infoHtml)
//        {
//            var innerContent = label + inputHtml;

// return innerContent + infoHtml + validation;
//        }

// protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
//        {
//            return "<div class=\"form-group\">" +
//                   Environment.NewLine + innerHtml + Environment.NewLine +
//                   "</div>";
//        }

// protected virtual Microsoft.AspNetCore.Razor.TagHelpers.TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
//        {
//            var inputTagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper(generator)
//            {
//                For = TagHelper.For,
//                InputTypeName = "text",
//                ViewContext = TagHelper.ViewContext,
//            };

// if (!TagHelper.Name.IsNullOrEmpty())
//                inputTagHelper.Name = TagHelper.Name;

// if (!TagHelper.Value.IsNullOrEmpty())
//                inputTagHelper.Value = TagHelper.Value;

// return inputTagHelper;
//        }

// protected virtual async Task<TagHelperOutput> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            var tagHelper = GetInputTagHelper(context, output);

// var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
//                GetInputAttributes(context, output),
//                context,
//                "input"
//            );

// AddDisabledAttribute(inputTagHelperOutput);
//            AddAutoFocusAttribute(inputTagHelperOutput);
//            AddFormControlClass(context, output, inputTagHelperOutput);
//            AddPlaceholderAttribute(inputTagHelperOutput);
//            AddInfoTextId(inputTagHelperOutput);

// return inputTagHelperOutput;
//        }

// private void AddFormControlClass(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTagHelperOutput)
//        {
//            inputTagHelperOutput.Attributes.Add("data-toggle", "colorpicker");
//            var val = TagHelper.For.Model as string;
//            if (!TagHelper.Value.IsNullOrEmpty())
//                val = TagHelper.Value;

// if (!val.IsNullOrEmpty())
//            {
//                inputTagHelperOutput.Attributes.AddIfNotExist("style", "background-color:" + val);
//                inputTagHelperOutput.Attributes.AddIfNotExist("data-color", val);
//            }
//            var className = "form-control";
//            inputTagHelperOutput.Attributes.AddClass(className + " " + GetSize(context, output));
//        }

// protected virtual void AddAutoFocusAttribute(TagHelperOutput inputTagHelperOutput)
//        {
//            if (TagHelper.AutoFocus && !inputTagHelperOutput.Attributes.ContainsName("data-auto-focus"))
//            {
//                inputTagHelperOutput.Attributes.Add("data-auto-focus", "true");
//            }
//        }

// protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
//        {
//            if (!inputTagHelperOutput.Attributes.ContainsName("disabled") &&
//                     (TagHelper.IsDisabled || cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Disabled == true))
//            {
//                inputTagHelperOutput.Attributes.Add("disabled", "");
//            }
//        }

// protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
//        {
//            if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
//                return;

// var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
//            if (attribute != null)
//            {
//                var placeholderLocalized = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Prompt);
//                if (!string.IsNullOrWhiteSpace(placeholderLocalized))
//                    inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
//            }
//        }

// protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
//        {
//            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");
//            if (idAttr == null)
//                return;

// var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
//            if (attribute != null)
//            {
//                var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
//                if (!string.IsNullOrWhiteSpace(description))
//                    inputTagHelperOutput.Attributes.Add("aria-describedby", description);
//            }
//        }

// protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
//        {
//            if (TagHelper.SuppressLabel)
//                return "";

// var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
//            if (uIHintAttribute != null && uIHintAttribute.LabelPosition == LabelPosition.Hidden)
//                return "";

// if (string.IsNullOrEmpty(TagHelper.Label))
//                return await GetLabelAsHtmlUsingTagHelperAsync(context, output) + GetRequiredSymbol(context, output);

// return "<label " + GetIdAttributeAsString(inputTag) + ">"
//                   + TagHelper.Label +
//                   "</label>" + GetRequiredSymbol(context, output);
//        }

// protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
//        {
//            if (!TagHelper.DisplayRequiredSymbol)
//                return "";

// return cachedModelAttributes?.GetAttribute<RequiredAttribute>() != null ? "<span> * </span>" : "";
//        }

// protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
//        {
//            var text = "";
//            if (!string.IsNullOrEmpty(TagHelper.InfoText))
//            {
//                text = TagHelper.InfoText;
//            }
//            else
//            {
//                var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
//                if (attribute != null)
//                {
//                    var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
//                    if (!string.IsNullOrWhiteSpace(description))
//                        text = description;
//                }
//            }

// if (string.IsNullOrEmpty(text))
//                return "";

// var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
//            return $"<small id=\"{idAttr?.Value}InfoText\" class=\"form-text text-muted\">{text}</small>";
//        }

// protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            var labelTagHelper = new LabelTagHelper(generator)
//            {
//                For = TagHelper.For,
//                ViewContext = TagHelper.ViewContext
//            };

// var attributeList = new TagHelperAttributeList();

// return await labelTagHelper.RenderAsync(attributeList, context, encoder, "label", TagMode.StartTagAndEndTag);
//        }

// protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
//        {
//            var groupPrefix = "group-";

// var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix));

// var attrList = new TagHelperAttributeList();

// foreach (var tagHelperAttribute in tagHelperAttributes)
//            {
//                attrList.Add(tagHelperAttribute);
//            }

// attrList.AddIfNotExist("type", "text");

// if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("name"))
//            {
//                attrList.Add("name", TagHelper.Name);
//            }

// if (!TagHelper.Value.IsNullOrEmpty() && !attrList.ContainsName("value"))
//            {
//                attrList.Add("value", TagHelper.Value);
//            }

// return attrList;
//        }

// protected virtual void LeaveOnlyGroupAttributes(TagHelperContext context, TagHelperOutput output)
//        {
//            var groupPrefix = "group-";
//            var tagHelperAttributes = output.Attributes.Where(a => a.Name.StartsWith(groupPrefix));

// output.Attributes.Clear();

// foreach (var tagHelperAttribute in tagHelperAttributes)
//            {
//                var nameWithoutPrefix = tagHelperAttribute.Name.Substring(groupPrefix.Length);
//                var newAttritube = new TagHelperAttribute(nameWithoutPrefix, tagHelperAttribute.Value);
//                output.Attributes.Add(newAttritube);
//            }
//        }

// protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
//        {
//            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
//            if (uIHintAttribute != null)
//                TagHelper.Size = uIHintAttribute.Size;

// switch (TagHelper.Size)
//            {
//                case FormControlSize.Small:
//                    return "custom-select-sm";
//                case FormControlSize.Medium:
//                    return "custom-select-md";
//                case FormControlSize.Large:
//                    return "custom-select-lg";
//                default:
//                    return "";
//            }
//        }

// protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
//        {
//            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

// return idAttr != null ? "for=\"" + idAttr.Value + "\"" : "";
//        }

// protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
//        {
//            var list = context.GetValue<List<FormGroupItem>>(FormGroupContents) ?? new List<FormGroupItem>();
//            suppress = list == null;

// if (list != null && !list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
//            {
//                list.Add(new FormGroupItem
//                {
//                    HtmlContent = html,
//                    Order = order,
//                    PropertyName = propertyName
//                });
//            }
//        }
//    }
// }
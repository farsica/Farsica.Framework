namespace Farsica.Framework.Mvc.ViewFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    public class HtmlGenerator : DefaultHtmlGenerator
    {
        public HtmlGenerator(IAntiforgery antiforgery, IOptions<MvcViewOptions> optionsAccessor, IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder, ValidationHtmlAttributeProvider validationAttributeProvider)
            : base(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
        {
        }

        public override TagBuilder GenerateSelect(ViewContext viewContext, ModelExplorer modelExplorer, string optionLabel, string expression, IEnumerable<SelectListItem> selectList, ICollection<string> currentValues, bool allowMultiple, object htmlAttributes)
        {
            htmlAttributes = (htmlAttributes as TagHelperAttributeList)?.ToDictionary(k => k.Name, v => v.Value) ?? htmlAttributes;
            return base.GenerateSelect(viewContext, modelExplorer, optionLabel, expression, selectList, currentValues, allowMultiple, htmlAttributes);
        }

        public override TagBuilder GenerateTextBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, string format, object htmlAttributes)
        {
            htmlAttributes = (htmlAttributes as TagHelperAttributeList)?.ToDictionary(k => k.Name, v => v.Value) ?? htmlAttributes;
            return base.GenerateTextBox(viewContext, modelExplorer, expression, value, format, htmlAttributes);
        }

        public override TagBuilder GenerateTextArea(ViewContext viewContext, ModelExplorer modelExplorer, string expression, int rows, int columns, object htmlAttributes)
        {
            htmlAttributes = (htmlAttributes as TagHelperAttributeList)?.ToDictionary(k => k.Name, v => v.Value) ?? htmlAttributes;
            return base.GenerateTextArea(viewContext, modelExplorer, expression, rows, columns, htmlAttributes);
        }

        internal static TagBuilder GenerateOption(SelectListItem item, string text)
        {
            return GenerateOption(item, text, item.Selected);
        }

        internal static TagBuilder GenerateOption(SelectListItem item, string text, bool selected)
        {
            var tagBuilder = new TagBuilder("option");
            tagBuilder.InnerHtml.SetContent(text);

            if (item.Value != null)
            {
                tagBuilder.Attributes["value"] = item.Value;
            }

            if (selected)
            {
                tagBuilder.Attributes["selected"] = "selected";
            }

            if (item.Disabled)
            {
                tagBuilder.Attributes["disabled"] = "disabled";
            }

            return tagBuilder;
        }

        protected override TagBuilder GenerateLink(string linkText, string url, object htmlAttributes)
        {
            var lst = url.Split("/", StringSplitOptions.RemoveEmptyEntries).ToList();
            if (lst?.Any() == true)
            {
                // if (!lst[0].Equals("api", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!Framework.Localization.CultureExtensions.GetAtomicValues().Any(t => t.Equals(lst[0], StringComparison.InvariantCultureIgnoreCase)))
                    {
                        lst.Insert(0, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    }
                }
            }
            else
            {
                lst.Insert(0, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            }

            return base.GenerateLink(linkText, string.Join("/", lst).Insert(0, "/"), htmlAttributes);
        }
    }
}

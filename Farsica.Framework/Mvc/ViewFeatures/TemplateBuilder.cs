namespace Farsica.Framework.Mvc.ViewFeatures
{
    using System;
    using System.Globalization;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

    internal class TemplateBuilder
    {
        private readonly IViewEngine viewEngine;
        private readonly IViewBufferScope bufferScope;
        private readonly ViewContext viewContext;
        private readonly ViewDataDictionary viewData;
        private readonly ModelExplorer modelExplorer;
        private readonly ModelMetadata metadata;
        private readonly string htmlFieldName;
        private readonly string templateName;
        private readonly bool readOnly;
        private readonly object additionalViewData;
        private object model;

        public TemplateBuilder(
            IViewEngine viewEngine,
            IViewBufferScope bufferScope,
            ViewContext viewContext,
            ViewDataDictionary viewData,
            ModelExplorer modelExplorer,
            string htmlFieldName,
            string templateName,
            bool readOnly,
            object additionalViewData)
        {
            this.viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
            this.bufferScope = bufferScope ?? throw new ArgumentNullException(nameof(bufferScope));
            this.viewContext = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
            this.viewData = viewData ?? throw new ArgumentNullException(nameof(viewData));
            this.modelExplorer = modelExplorer ?? throw new ArgumentNullException(nameof(modelExplorer));
            this.htmlFieldName = htmlFieldName;
            this.templateName = templateName;
            this.readOnly = readOnly;
            this.additionalViewData = additionalViewData;

            model = modelExplorer.Model;
            metadata = modelExplorer.Metadata;
        }

        public IHtmlContent Build()
        {
            if (metadata.ConvertEmptyStringToNull && string.Empty.Equals(model))
            {
                model = null;
            }

            // Normally this shouldn't happen, unless someone writes their own custom Object templates which
            // don't check to make sure that the object hasn't already been displayed
            if (this.viewData.TemplateInfo.Visited(modelExplorer))
            {
                return HtmlString.Empty;
            }

            // Create VDD of type object so any model type is allowed.
            var viewData = new ViewDataDictionary<object>(this.viewData)
            {
                // Create a new ModelExplorer in order to preserve the model metadata of the original _viewData even
                // though _model may have been reset to null. Otherwise we might lose track of the model type /property.
                ModelExplorer = modelExplorer.GetExplorerForModel(model),
            };

            var formatString = readOnly ?
                viewData.ModelMetadata.DisplayFormatString :
                viewData.ModelMetadata.EditFormatString;

            var formattedModelValue = model;
            if (model == null)
            {
                if (readOnly)
                {
                    formattedModelValue = metadata.NullDisplayText;
                }
            }
            else if (!string.IsNullOrEmpty(formatString))
            {
                formattedModelValue = string.Format(CultureInfo.CurrentCulture, formatString, model);
            }
            else if (string.Equals("week", templateName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("week", viewData.ModelMetadata.DataTypeName, StringComparison.OrdinalIgnoreCase))
            {
                // "week" is a new HTML5 input type that only will be rendered in Rfc3339 mode
                formattedModelValue = FormatWeekHelper.GetFormattedWeek(modelExplorer);
            }
            else if (viewData.ModelMetadata.IsEnum && model is Enum modelEnum)
            {
                // Cover the case where the model is an enum and we want the string value of it
                var value = modelEnum.ToString("d");
                var enumGrouped = viewData.ModelMetadata.EnumGroupedDisplayNamesAndValues;
                foreach (var kvp in enumGrouped)
                {
                    if (kvp.Value == value)
                    {
                        // Creates a ModelExplorer with the same Metadata except that the Model is a string instead of an Enum
                        formattedModelValue = kvp.Key.Name;
                        break;
                    }
                }
            }

            viewData.TemplateInfo.FormattedModelValue = formattedModelValue;
            viewData.TemplateInfo.HtmlFieldPrefix = this.viewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);

            if (additionalViewData != null)
            {
                foreach (var kvp in Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.ObjectToDictionary(additionalViewData))
                {
                    viewData[kvp.Key] = kvp.Value;
                }
            }

            var visitedObjectsKey = model ?? modelExplorer.ModelType;
            viewData.TemplateInfo.AddVisited(visitedObjectsKey);

            var templateRenderer = new TemplateRenderer(
                viewEngine,
                bufferScope,
                viewContext,
                viewData,
                templateName,
                readOnly);

            return templateRenderer.Render();
        }
    }
}

namespace Farsica.Framework.Mvc.ViewFeatures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Mvc.ViewFeatures.Buffers;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
    using Microsoft.Extensions.DependencyInjection;

    internal class TemplateRenderer(
        IViewEngine viewEngine,
        IViewBufferScope bufferScope,
        ViewContext viewContext,
        ViewDataDictionary viewData,
        string? templateName,
        bool readOnly)
    {
        public const string IEnumerableOfIFormFileName = "IEnumerable`" + nameof(IFormFile);
        private const string DisplayTemplateViewPath = "DisplayTemplates";
        private const string EditorTemplateViewPath = "EditorTemplates";

        private static readonly Dictionary<string, Func<IHtmlHelper, IHtmlContent>> DefaultDisplayActions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "Collection", DefaultDisplayTemplates.CollectionTemplate },
                { "EmailAddress", DefaultDisplayTemplates.EmailAddressTemplate },
                { "HiddenInput", DefaultDisplayTemplates.HiddenInputTemplate },
                { "Html", DefaultDisplayTemplates.HtmlTemplate },
                { "Text", DefaultDisplayTemplates.StringTemplate },
                { "Url", DefaultDisplayTemplates.UrlTemplate },
                { nameof(Boolean), DefaultDisplayTemplates.BooleanTemplate },
                { nameof(Decimal), DefaultDisplayTemplates.DecimalTemplate },
                { nameof(String), DefaultDisplayTemplates.StringTemplate },
                { nameof(Object), DefaultDisplayTemplates.ObjectTemplate },
            };

        private static readonly Dictionary<string, Func<IHtmlHelper, IHtmlContent>> DefaultEditorActions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "Collection", DefaultEditorTemplates.CollectionTemplate },
                { "EmailAddress", DefaultEditorTemplates.EmailAddressInputTemplate },
                { "HiddenInput", DefaultEditorTemplates.HiddenInputTemplate },
                { "MultilineText", DefaultEditorTemplates.MultilineTemplate },
                { "Password", DefaultEditorTemplates.PasswordTemplate },
                { "PhoneNumber", DefaultEditorTemplates.PhoneNumberInputTemplate },
                { "Text", DefaultEditorTemplates.StringTemplate },
                { "Url", DefaultEditorTemplates.UrlInputTemplate },
                { "Date", DefaultEditorTemplates.DateInputTemplate },
                { "DateTime", DefaultEditorTemplates.DateTimeLocalInputTemplate },
                { "DateTime-local", DefaultEditorTemplates.DateTimeLocalInputTemplate },
                { nameof(DateTimeOffset), DefaultEditorTemplates.DateTimeOffsetTemplate },
                { "Time", DefaultEditorTemplates.TimeInputTemplate },
                { "Month", DefaultEditorTemplates.MonthInputTemplate },
                { "Week", DefaultEditorTemplates.WeekInputTemplate },
                { nameof(Byte), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(SByte), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(Int16), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(UInt16), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(Int32), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(UInt32), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(Int64), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(UInt64), DefaultEditorTemplates.NumberInputTemplate },
                { nameof(Boolean), DefaultEditorTemplates.BooleanTemplate },
                { nameof(Decimal), DefaultEditorTemplates.DecimalTemplate },
                { nameof(String), DefaultEditorTemplates.StringTemplate },
                { nameof(Object), DefaultEditorTemplates.ObjectTemplate },
                { nameof(IFormFile), DefaultEditorTemplates.FileInputTemplate },
                { IEnumerableOfIFormFileName, DefaultEditorTemplates.FileCollectionInputTemplate },
            };

        private readonly IViewEngine viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
        private readonly IViewBufferScope bufferScope = bufferScope ?? throw new ArgumentNullException(nameof(bufferScope));
        private readonly ViewContext viewContext = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
        private readonly ViewDataDictionary viewData = viewData ?? throw new ArgumentNullException(nameof(viewData));
        private readonly string? templateName = templateName;
        private readonly bool readOnly = readOnly;

        public static IEnumerable<string> GetTypeNames(ModelMetadata modelMetadata, Type fieldType)
        {
            // Not returning type name here for IEnumerable<IFormFile> since we will be returning
            // a more specific name, IEnumerableOfIFormFileName.
            if (typeof(IEnumerable<IFormFile>) != fieldType)
            {
                yield return fieldType.Name;
            }

            if (fieldType == typeof(string))
            {
                // Nothing more to provide
                yield break;
            }
            else if (!modelMetadata.IsComplexType)
            {
                // IsEnum is false for the Enum class itself
                if (fieldType.IsEnum)
                {
                    // Same as fieldType.BaseType.Name in this case
                    yield return "Enum";
                }
                else if (fieldType == typeof(DateTimeOffset))
                {
                    yield return "DateTime";
                }

                yield return "String";
                yield break;
            }
            else if (!fieldType.IsInterface)
            {
                var type = fieldType;
                while (true)
                {
                    type = type.BaseType;
                    if (type is null || type == typeof(object))
                    {
                        break;
                    }

                    yield return type.Name;
                }
            }

            if (typeof(IEnumerable).IsAssignableFrom(fieldType))
            {
                if (typeof(IEnumerable<IFormFile>).IsAssignableFrom(fieldType))
                {
                    yield return IEnumerableOfIFormFileName;

                    // Specific name has already been returned, now return the generic name.
                    if (typeof(IEnumerable<IFormFile>) == fieldType)
                    {
                        yield return fieldType.Name;
                    }
                }

                yield return "Collection";
            }
            else if (typeof(IFormFile) != fieldType && typeof(IFormFile).IsAssignableFrom(fieldType))
            {
                yield return nameof(IFormFile);
            }

            yield return "Object";
        }

        public IHtmlContent Render()
        {
            var defaultActions = GetDefaultActions();
            var modeViewPath = readOnly ? DisplayTemplateViewPath : EditorTemplateViewPath;

            foreach (var viewName in GetViewNames())
            {
                var viewEngineResult = viewEngine.GetView(viewContext.ExecutingFilePath, viewName, isMainPage: false);
                if (!viewEngineResult.Success)
                {
                    var fullViewName = modeViewPath + "/" + viewName;
                    viewEngineResult = viewEngine.FindView(viewContext, fullViewName, isMainPage: false);
                }

                if (viewEngineResult.Success)
                {
                    var viewBuffer = new ViewBuffer(bufferScope, viewName, ViewBuffer.PartialViewPageSize);
                    using var writer = new ViewBufferTextWriter(viewBuffer, viewContext.Writer.Encoding);

                    // Forcing synchronous behavior so users don't have to await templates.
                    var view = viewEngineResult.View;
                    using (view as IDisposable)
                    {
                        var viewContext = new ViewContext(this.viewContext, viewEngineResult.View, viewData, writer);
                        var renderTask = viewEngineResult.View.RenderAsync(viewContext);
                        renderTask.GetAwaiter().GetResult();
                        return viewBuffer;
                    }
                }

                if (defaultActions.TryGetValue(viewName, out var defaultAction))
                {
                    return defaultAction(MakeHtmlHelper(viewContext, viewData));
                }
            }

            throw new InvalidOperationException("NoTemplate");
        }

        private static IHtmlHelper MakeHtmlHelper(ViewContext viewContext, ViewDataDictionary viewData)
        {
            var newHelper = viewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper>();

            if (newHelper is IViewContextAware contextable)
            {
                var newViewContext = new ViewContext(viewContext, viewContext.View, viewData, viewContext.Writer);
                contextable.Contextualize(newViewContext);
            }

            return newHelper;
        }

        private Dictionary<string, Func<IHtmlHelper, IHtmlContent>> GetDefaultActions()
        {
            return readOnly ? DefaultDisplayActions : DefaultEditorActions;
        }

        private IEnumerable<string> GetViewNames()
        {
            var metadata = viewData.ModelMetadata;
            var templateHints = new[]
            {
                templateName,
                metadata.TemplateHint,
                metadata.DataTypeName,
            };

            foreach (var templateHint in templateHints.Where(s => !string.IsNullOrEmpty(s)))
            {
                yield return templateHint;
            }

            // We don't want to search for Nullable<T>, we want to search for T (which should handle both T and
            // Nullable<T>).
            var fieldType = metadata.UnderlyingOrModelType;
            foreach (var typeName in GetTypeNames(metadata, fieldType))
            {
                yield return typeName;
            }
        }
    }
}

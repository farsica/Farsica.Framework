namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Button;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.DependencyInjection;

    [Injectable]
    public class FormTagHelperService(HtmlEncoder htmlEncoder, IHtmlGenerator htmlGenerator, IServiceProvider serviceProvider) : TagHelperService<FormTagHelper>
    {
        private readonly HtmlEncoder htmlEncoder = htmlEncoder;
        private readonly IHtmlGenerator htmlGenerator = htmlGenerator;
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var list = InitilizeFormGroupContentsContext(context, output);

            NormalizeTagMode(context, output);

            var childContent = (await output.GetChildContentAsync()).GetContent();

            await ConvertToMvcForm(context, output);

            await ProcessFieldsAsync(context, output);

            RemoveFormGroupItemsNotInModel(context, output, list);

            SetContent(context, output, list, childContent);

            output.Attributes.AddIfNotExist("method", TagHelper?.Method);

            await SetSubmitButton(context, output);
        }

        private static void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "form";
        }

        private static List<FormGroupItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<FormGroupItem>();
            context.Items[FormGroupContents] = items;
            return items;
        }

        private static ModelExpression ModelExplorerToModelExpressionConverter(ModelExplorer explorer)
        {
            var temp = explorer;
            var propertyName = explorer.Metadata.PropertyName;

            while (temp?.Container?.Metadata?.PropertyName is not null)
            {
                temp = temp.Container;
                propertyName = temp.Metadata.PropertyName + "." + propertyName;
            }

            return new ModelExpression(propertyName, explorer);
        }

        private static bool IsCsharpClassOrPrimitive(Type type)
        {
            if (type is null)
            {
                return false;
            }

            return type.IsPrimitive ||
                   type.IsValueType ||
                   type == typeof(string) ||
                   type == typeof(Guid) ||
                   type == typeof(DateTime) ||
                   type == typeof(ValueType) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(DateTimeOffset) ||
                   type.IsEnum;
        }

        private static bool IsSelectGroup(TagHelperContext context, ModelExpression model)
        {
            return model.ModelExplorer?.Metadata?.IsEnum == true
                || model.ModelExplorer?.Metadata?.ElementType?.IsEnum == true
                || model.ModelExplorer?.GetAttribute<SelectItemsAttribute>() is not null;
        }

        private static bool IsListOfCsharpClassOrPrimitive(Type type)
        {
            var genericType = type.GenericTypeArguments.FirstOrDefault();

            if (genericType is null || !IsCsharpClassOrPrimitive(genericType))
            {
                return false;
            }

            return type.ToString().StartsWith("System.Collections.Generic.IEnumerable`") || type.ToString().StartsWith("System.Collections.Generic.List`");
        }

        private static void SetContent(TagHelperContext context, TagHelperOutput output, List<FormGroupItem> items, string? childContent)
        {
            var contentBuilder = new StringBuilder(string.Empty);

            var orderedItems = items.OrderBy(o => o.Order);
            foreach (var item in orderedItems)
            {
                contentBuilder.AppendLine(item.HtmlContent);
            }

            if (childContent?.Contains(FormContentPlaceHolder) == true)
            {
                output.Content.SetHtmlContent(childContent.Replace(FormContentPlaceHolder, contentBuilder.ToString()));
            }
            else
            {
                output.Content.SetHtmlContent(contentBuilder + childContent);
            }
        }

        private async Task ConvertToMvcForm(TagHelperContext context, TagHelperOutput output)
        {
            var formTagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper(htmlGenerator)
            {
                Action = TagHelper.Action,
                Controller = TagHelper.Controller,
                Area = TagHelper.Area,
                Page = TagHelper.Page,
                PageHandler = TagHelper.PageHandler,
                Antiforgery = TagHelper.Antiforgery,
                Fragment = TagHelper.Fragment,
                Route = TagHelper.Route,
                Method = null,
                RouteValues = TagHelper.RouteValues,
                ViewContext = TagHelper.ViewContext,
            };

            var formTagOutput = await formTagHelper.ProcessAndGetOutputAsync(output.Attributes, context, "form", TagMode.StartTagAndEndTag);

            await formTagOutput.GetChildContentAsync();

            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + formTagOutput.PostContent.GetContent());
            output.PreContent.SetHtmlContent(output.PreContent.GetContent() + formTagOutput.PreContent.GetContent());
        }

        private async Task SetSubmitButton(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplaySubmitButton.GetValueOrDefault())
            {
                return;
            }

            var buttonHtml = await ProcessSubmitButtonAndGetContentAsync(context, output);
            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + buttonHtml);
        }

        private async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output)
        {
            var models = GetModels(context, output);
            for (int i = 0; i < models.Count; i++)
            {
                var model = models[i];

                if (IsSelectGroup(context, model))
                {
                    await ProcessSelectGroupAsync(context, output, model);
                }
                else
                {
                    await ProcessInputGroupAsync(context, output, model);
                }
            }
        }

        private async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
        {
            var dataTypeAttribute = model.ModelExplorer.GetAttribute<DataTypeAttribute>();
            var selectTagHelper = dataTypeAttribute is not null && dataTypeAttribute.ElementDataType == ElementDataType.RadioButton ?
                GetRadioInputTagHelper(model) :
                GetSelectTagHelper(model);

            await selectTagHelper.RenderAsync([], context, htmlEncoder, "div", TagMode.StartTagAndEndTag);
        }

        private async Task ProcessInputGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
        {
            var inputTagHelper = serviceProvider.GetRequiredService<InputTagHelper>();
            inputTagHelper.For = model;
            inputTagHelper.ViewContext = TagHelper.ViewContext;
            inputTagHelper.DisplayRequiredSymbol = TagHelper.DisplayRequiredSymbol;

            await inputTagHelper.RenderAsync([], context, htmlEncoder, "div", TagMode.StartTagAndEndTag);
        }

        private void RemoveFormGroupItemsNotInModel(TagHelperContext context, TagHelperOutput output, List<FormGroupItem> items)
        {
            var models = GetModels(context, output);
            items.RemoveAll(t => models.All(m => !m.Name.Equals(t.PropertyName, StringComparison.InvariantCultureIgnoreCase)));
        }

        private TagHelper GetSelectTagHelper(ModelExpression model)
        {
            var selectTagHelper = serviceProvider.GetRequiredService<SelectTagHelper>();
            selectTagHelper.For = model;
            selectTagHelper.Items = null;
            selectTagHelper.ViewContext = TagHelper.ViewContext;
            return selectTagHelper;
        }

        private TagHelper GetRadioInputTagHelper(ModelExpression model)
        {
            var uIHintAttribute = model.ModelExplorer.GetAttribute<UIHintAttribute>();
            var radioInputTagHelper = serviceProvider.GetRequiredService<RadioInputTagHelper>();
            radioInputTagHelper.For = model;
            radioInputTagHelper.Items = null;
            radioInputTagHelper.Inline = uIHintAttribute?.Inline;
            radioInputTagHelper.Disabled = uIHintAttribute?.Disabled;
            radioInputTagHelper.ViewContext = TagHelper.ViewContext;
            return radioInputTagHelper;
        }

        private async Task<string?> ProcessSubmitButtonAndGetContentAsync(TagHelperContext context, TagHelperOutput output)
        {
            var buttonTagHelper = serviceProvider.GetRequiredService<ButtonTagHelper>();
            var attributes = new TagHelperAttributeList { new TagHelperAttribute("type", "submit") };
            buttonTagHelper.Text = "Submit";
            buttonTagHelper.ButtonType = ButtonType.Primary;

            return await buttonTagHelper.RenderAsync(attributes, context, htmlEncoder, "button", TagMode.StartTagAndEndTag);
        }

        private List<ModelExpression> GetModels(TagHelperContext context, TagHelperOutput output)
        {
            return TagHelper.For.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively);
        }

        private List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
        {
            var scaffoldColumnAttribute = model.GetAttribute<ScaffoldColumnAttribute>();
            if (scaffoldColumnAttribute is not null && !scaffoldColumnAttribute.Scaffold)
            {
                return list;
            }

            if (IsCsharpClassOrPrimitive(model.ModelType) || IsListOfCsharpClassOrPrimitive(model.ModelType))
            {
                list.Add(ModelExplorerToModelExpressionConverter(model));
                return list;
            }

            if (model.ModelType == typeof(List<SelectListItem>) || model.ModelType == typeof(IEnumerable<SelectListItem>))
            {
                return list;
            }

            return model.Properties.Aggregate(list, ExploreModelsRecursively);
        }
    }
}

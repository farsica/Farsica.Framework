namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using System;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Button;
    using Farsica.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.DependencyInjection;

    [DataAnnotation.Injectable]
    public class DropdownButtonTagHelperService(
        HtmlEncoder htmlEncoder,
        IServiceProvider serviceProvider) : TagHelperService<DropdownButtonTagHelper>
    {
        private readonly HtmlEncoder htmlEncoder = htmlEncoder;
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();

            var buttonsAsHtml = await GetButtonsAsHtmlAsync(context, output, content);

            output.PreElement.SetHtmlContent(buttonsAsHtml);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetContent(string.Empty);
            output.Attributes.Clear();
        }

        protected virtual async Task<string?> GetButtonsAsHtmlAsync(TagHelperContext context, TagHelperOutput output,
            TagHelperContent content)
        {
            var buttonBuilder = new StringBuilder(string.Empty);

            var mainButton = await GetMainButtonAsync(context, output, content);

            buttonBuilder.AppendLine(mainButton);

            if (TagHelper.DropdownStyle == DropdownStyle.Split)
            {
                var splitButton = await GetSplitButtonAsync(context, output);

                buttonBuilder.AppendLine(splitButton);
            }

            return buttonBuilder.ToString();
        }

        protected virtual async Task<string?> GetMainButtonAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent content)
        {
            var buttonTagHelper = serviceProvider.GetRequiredService<ButtonTagHelper>();

            buttonTagHelper.Icon = TagHelper.Icon;
            buttonTagHelper.Text = TagHelper.Text;
            buttonTagHelper.IconType = TagHelper.IconType;
            buttonTagHelper.Size = TagHelper.Size;
            buttonTagHelper.ButtonType = TagHelper.ButtonType;
            var attributes = GetAttributesForMainButton(context, output);

            var buttonTag = await buttonTagHelper.ProcessAndGetOutputAsync(attributes, context, "button", TagMode.StartTagAndEndTag);

            buttonTag.PreContent.SetHtmlContent(content.GetContent());

            if ((TagHelper.NavLink ?? false) || (TagHelper.Link ?? false))
            {
                var linkTag = ConvertButtonToLink(buttonTag);
                return linkTag.Render(htmlEncoder);
            }

            return buttonTag.Render(htmlEncoder);
        }

        protected virtual async Task<string?> GetSplitButtonAsync(TagHelperContext context, TagHelperOutput output)
        {
            var buttonTagHelper = serviceProvider.GetRequiredService<ButtonTagHelper>();

            buttonTagHelper.Size = TagHelper.Size;
            buttonTagHelper.ButtonType = TagHelper.ButtonType;
            var attributes = GetAttributesForSplitButton(context, output);

            return await buttonTagHelper.RenderAsync(attributes, context, htmlEncoder, "button", TagMode.StartTagAndEndTag);
        }

        protected virtual TagHelperAttributeList GetAttributesForMainButton(TagHelperContext context, TagHelperOutput output)
        {
            var attributes = new TagHelperAttributeList();

            foreach (var tagHelperAttribute in output.Attributes)
            {
                attributes.Add(tagHelperAttribute);
            }

            if (TagHelper.DropdownStyle != DropdownStyle.Split)
            {
                attributes.AddClass("dropdown-toggle");
                attributes.Add("data-toggle", "dropdown");
                attributes.Add("aria-haspopup", "true");
                attributes.Add("aria-expanded", "false");
            }

            return attributes;
        }

        protected virtual TagHelperAttributeList GetAttributesForSplitButton(TagHelperContext context, TagHelperOutput output)
        {
            var attributes = new TagHelperAttributeList
            {
                { "data-toggle", "dropdown" },
                { "aria-haspopup", "true" },
                { "aria-expanded", "false" },
            };

            attributes.AddClass("dropdown-toggle");
            attributes.AddClass("dropdown-toggle-split");

            return attributes;
        }

        protected virtual TagHelperOutput ConvertButtonToLink(TagHelperOutput buttonTag)
        {
            buttonTag.TagName = "a";
            buttonTag.Attributes.RemoveAll("type");
            buttonTag.Attributes.Add("roles", "button");
            buttonTag.Attributes.Add("href", "#");

            if (TagHelper.NavLink ?? false)
            {
                buttonTag.Attributes.AddClass("nav-link");
            }

            return buttonTag;
        }
    }
}

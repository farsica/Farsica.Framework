namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Farsica.Framework.Core.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public abstract class ButtonTagHelperServiceBase<TTagHelper> : TagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IButtonTagHelperBase
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            NormalizeTagMode(context, output);
            AddClasses(context, output);
            AddIcon(context, output);
            AddText(context, output);
            AddDisabled(context, output);
        }

        protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("btn");

            if (TagHelper.ButtonType != ButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButtonType.ToString().ToLowerInvariant().Replace("_", "-"));
            }

            if (TagHelper.Size != ButtonSize.Default)
            {
                output.Attributes.AddClass(TagHelper.Size.ToClassName());
            }
        }

        protected virtual void AddIcon(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Icon.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Content.AppendHtml($"<i class=\"{GetIconClass(context, output)}\"></i> ");
        }

        protected virtual string GetIconClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.IconType)
            {
                case FontIconType.FontAwesome:
                    return "fas fa-" + TagHelper.Icon;
                default:
                    return TagHelper.Icon;
            }
        }

        protected virtual void AddText(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Content.AppendHtml(TagHelper.Text);
        }

        protected virtual void AddDisabled(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Disabled ?? false)
            {
                output.Attributes.Add("disabled", "disabled");
            }
        }
    }
}
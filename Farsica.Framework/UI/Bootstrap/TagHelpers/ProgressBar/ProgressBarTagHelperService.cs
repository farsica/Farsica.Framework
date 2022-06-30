namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    using System;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ProgressBarTagHelperService : TagHelperService<ProgressBarTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetParentElement(context, output);

            output.Attributes.AddClass("progress-bar");
            output.Attributes.Add("role", "progressbar");

            SetAnimationClass(context, output);
            SetStripClass(context, output);
            SetTypeClass(context, output);
            SetValues(context, output);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual void SetValues(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("style", "width: " + CalculateStyleWidth() + "%");
            output.Attributes.Add("aria-valuenow", TagHelper.Value);
            output.Attributes.Add("aria-valuemin", TagHelper.MinValue);
            output.Attributes.Add("aria-valuemax", TagHelper.MaxValue);
        }

        protected virtual void SetAnimationClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Animation ?? false)
            {
                output.Attributes.AddClass("progress-bar-animated");
            }
        }

        protected virtual void SetStripClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Strip ?? false)
            {
                output.Attributes.AddClass("progress-bar-striped");
            }
        }

        protected virtual void SetTypeClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Type != ProgressBarType.Default)
            {
                output.Attributes.AddClass("bg-" + TagHelper.Type.ToString().ToLowerInvariant());
            }
        }

        protected virtual void SetParentElement(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "frb-progress-part")
            {
                return;
            }

            output.PreElement.SetHtmlContent("<div class=\"progress\">" + Environment.NewLine);
            output.PostElement.SetHtmlContent(Environment.NewLine + "</div>");
        }

        protected virtual int CalculateStyleWidth()
        {
            return (int)(((double)TagHelper.Value - TagHelper.MinValue) * (100 / (TagHelper.MaxValue - TagHelper.MinValue)));
        }
    }
}
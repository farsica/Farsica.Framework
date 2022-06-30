namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Grid
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class RowTagHelperService : TagHelperService<RowTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "frb-row")
            {
                output.Attributes.AddClass("row");
            }

            if (output.TagName == "frb-form-row")
            {
                output.Attributes.AddClass("form-row");
            }

            output.TagName = "div";

            ProcessVerticalAlign(output);
            ProcessHorizontalAlign(output);
            ProcessGutters(output);
        }

        protected virtual void ProcessVerticalAlign(TagHelperOutput output)
        {
            if (TagHelper.VerticalAlign == VerticalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("align-items-" + TagHelper.VerticalAlign.ToString().ToLowerInvariant());
        }

        protected virtual void ProcessHorizontalAlign(TagHelperOutput output)
        {
            if (TagHelper.HorizontalAlign == HorizontalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("justify-content-" + TagHelper.HorizontalAlign.ToString().ToLowerInvariant());
        }

        protected virtual void ProcessGutters(TagHelperOutput output)
        {
            if (TagHelper.Gutters ?? true)
            {
                return;
            }

            output.Attributes.AddClass("no-gutters");
        }
    }
}
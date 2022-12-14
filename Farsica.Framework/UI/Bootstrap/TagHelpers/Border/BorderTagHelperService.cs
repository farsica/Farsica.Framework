namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class BorderTagHelperService : TagHelperService<BorderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var borderAttributeAsString = TagHelper.Border.ToString().ToLowerInvariant();

            var borderClass = "border" + GetBorderDirection(context, output, borderAttributeAsString) + GetExtensionIfBorderIsSubtractive(context, output, borderAttributeAsString);

            output.Attributes.AddClass(borderClass);

            SetBorderType(context, output, borderAttributeAsString);
        }

        protected virtual string? GetBorderDirection(TagHelperContext context, TagHelperOutput output, string? borderAttributeAsString)
        {
            if (borderAttributeAsString?.Contains("top") is true)
            {
                return "-top";
            }

            if (borderAttributeAsString?.Contains("right") is true)
            {
                return "-right";
            }

            if (borderAttributeAsString?.Contains("left") is true)
            {
                return "-left";
            }

            if (borderAttributeAsString?.Contains("bottom") is true)
            {
                return "-bottom";
            }

            return string.Empty;
        }

        protected virtual string? GetExtensionIfBorderIsSubtractive(TagHelperContext context, TagHelperOutput output, string? borderAttributeAsString)
        {
            if (borderAttributeAsString?.Contains("_0") is true)
            {
                return "-0";
            }

            return string.Empty;
        }

        protected virtual void SetBorderType(TagHelperContext context, TagHelperOutput output, string? borderAttributeAsString)
        {
            if (borderAttributeAsString?.Contains("primary") is true)
            {
                output.Attributes.AddClass("border-primary");
            }

            if (borderAttributeAsString?.Contains("secondary") is true)
            {
                output.Attributes.AddClass("border-secondary");
            }

            if (borderAttributeAsString?.Contains("success") is true)
            {
                output.Attributes.AddClass("border-success");
            }

            if (borderAttributeAsString?.Contains("danger") is true)
            {
                output.Attributes.AddClass("border-danger");
            }

            if (borderAttributeAsString?.Contains("warning") is true)
            {
                output.Attributes.AddClass("border-warning");
            }

            if (borderAttributeAsString?.Contains("info") is true)
            {
                output.Attributes.AddClass("border-info");
            }

            if (borderAttributeAsString?.Contains("light") is true)
            {
                output.Attributes.AddClass("border-light");
            }

            if (borderAttributeAsString?.Contains("dark") is true)
            {
                output.Attributes.AddClass("border-dark");
            }

            if (borderAttributeAsString?.Contains("white") is true)
            {
                output.Attributes.AddClass("border-white");
            }
        }
    }
}

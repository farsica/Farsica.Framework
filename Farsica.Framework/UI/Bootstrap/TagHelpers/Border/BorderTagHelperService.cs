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
            if (borderAttributeAsString?.Contains("top") == true)
            {
                return "-top";
            }

            if (borderAttributeAsString?.Contains("right") == true)
            {
                return "-right";
            }

            if (borderAttributeAsString?.Contains("left") == true)
            {
                return "-left";
            }

            if (borderAttributeAsString?.Contains("bottom") == true)
            {
                return "-bottom";
            }

            return string.Empty;
        }

        protected virtual string? GetExtensionIfBorderIsSubtractive(TagHelperContext context, TagHelperOutput output, string? borderAttributeAsString)
        {
            if (borderAttributeAsString?.Contains("_0") == true)
            {
                return "-0";
            }

            return string.Empty;
        }

        protected virtual void SetBorderType(TagHelperContext context, TagHelperOutput output, string? borderAttributeAsString)
        {
            if (borderAttributeAsString?.Contains("primary") == true)
            {
                output.Attributes.AddClass("border-primary");
            }

            if (borderAttributeAsString?.Contains("secondary") == true)
            {
                output.Attributes.AddClass("border-secondary");
            }

            if (borderAttributeAsString?.Contains("success") == true)
            {
                output.Attributes.AddClass("border-success");
            }

            if (borderAttributeAsString?.Contains("danger") == true)
            {
                output.Attributes.AddClass("border-danger");
            }

            if (borderAttributeAsString?.Contains("warning") == true)
            {
                output.Attributes.AddClass("border-warning");
            }

            if (borderAttributeAsString?.Contains("info") == true)
            {
                output.Attributes.AddClass("border-info");
            }

            if (borderAttributeAsString?.Contains("light") == true)
            {
                output.Attributes.AddClass("border-light");
            }

            if (borderAttributeAsString?.Contains("dark") == true)
            {
                output.Attributes.AddClass("border-dark");
            }

            if (borderAttributeAsString?.Contains("white") == true)
            {
                output.Attributes.AddClass("border-white");
            }
        }
    }
}

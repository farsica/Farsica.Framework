namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Border
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class RoundedTagHelperService : TagHelperService<RoundedTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var roundedClass = "rounded";

            if (TagHelper.Rounded != RoundedType.Default)
            {
                roundedClass += "-" + TagHelper.Rounded.ToString().ToLowerInvariant().Replace("_", string.Empty);
            }

            output.Attributes.AddClass(roundedClass);
        }
    }
}
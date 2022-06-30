namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown")]
    public class DropdownTagHelper : TagHelper<DropdownTagHelper, DropdownTagHelperService>
    {
        public DropdownTagHelper(DropdownTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        public DropdownDirection Direction { get; set; } = DropdownDirection.Down;
    }
}

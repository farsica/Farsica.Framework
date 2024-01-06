namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown")]
    public class DropdownTagHelper(DropdownTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<DropdownTagHelper, DropdownTagHelperService>(tagHelperService, optionsAccessor)
    {
        public DropdownDirection Direction { get; set; } = DropdownDirection.Down;
    }
}

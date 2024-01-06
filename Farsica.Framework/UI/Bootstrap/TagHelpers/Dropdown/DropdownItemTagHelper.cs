namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-item")]
    public class DropdownItemTagHelper(DropdownItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<DropdownItemTagHelper, DropdownItemTagHelperService>(tagHelperService, optionsAccessor)
    {
        public bool? Active { get; set; }

        public bool? Disabled { get; set; }
    }
}

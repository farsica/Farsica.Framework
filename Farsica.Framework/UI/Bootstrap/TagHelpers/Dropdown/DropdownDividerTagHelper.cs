namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-divider")]
    public class DropdownDividerTagHelper(DropdownDividerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<DropdownDividerTagHelper, DropdownDividerTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-header")]
    public class DropdownHeaderTagHelper(DropdownHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<DropdownHeaderTagHelper, DropdownHeaderTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

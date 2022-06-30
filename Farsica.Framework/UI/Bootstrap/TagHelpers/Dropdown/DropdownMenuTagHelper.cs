namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-menu")]
    public class DropdownMenuTagHelper : TagHelper<DropdownMenuTagHelper, DropdownMenuTagHelperService>
    {
        public DropdownMenuTagHelper(DropdownMenuTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        public DropdownAlign Align { get; set; } = DropdownAlign.Left;
    }
}

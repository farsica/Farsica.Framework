namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab-dropdown")]
    public class TabDropdownTagHelper(TabDropdownTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<TabDropdownTagHelper, TabDropdownTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-title")]
        public string? Title { get; set; }
    }
}

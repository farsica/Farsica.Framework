namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab")]
    public class TabTagHelper : TagHelper<TabTagHelper, TabTagHelperService>
    {
        public TabTagHelper(TabTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-parent-dropdown-name")]
        public string ParentDropdownName { get; set; }
    }
}

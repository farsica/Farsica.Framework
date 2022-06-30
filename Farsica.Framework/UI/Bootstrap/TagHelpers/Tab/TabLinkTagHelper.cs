namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab-link", TagStructure = TagStructure.WithoutEndTag)]
    public class TabLinkTagHelper : TagHelper<TabLinkTagHelper, TabLinkTagHelperService>
    {
        public TabLinkTagHelper(TabLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-parent-dropdown-name")]
        public string ParentDropdownName { get; set; }

        [HtmlAttributeName("frb-href")]
        public string Href { get; set; }
    }
}

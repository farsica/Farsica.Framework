namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-list-group-item")]
    public class ListGroupItemTagHelper : TagHelper<ListGroupItemTagHelper, ListGroupItemTagHelperService>
    {
        public ListGroupItemTagHelper(ListGroupItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        [HtmlAttributeName("frb-href")]
        public string Href { get; set; }

        [HtmlAttributeName("frb-tag-type")]
        public ListItemTagType TagType { get; set; } = ListItemTagType.Default;

        [HtmlAttributeName("frb-type")]
        public ListItemType Type { get; set; } = ListItemType.Default;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-list-group")]
    public class ListGroupTagHelper : TagHelper<ListGroupTagHelper, ListGroupTagHelperService>
    {
        public ListGroupTagHelper(ListGroupTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-flush")]
        public bool? Flush { get; set; }
    }
}

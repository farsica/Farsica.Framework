namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-radio")]
    public class RadioInputTagHelper : TagHelper<RadioInputTagHelper, RadioInputTagHelperService>
    {
        public RadioInputTagHelper(RadioInputTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-label")]
        public string? Label { get; set; }

        [HtmlAttributeName("frb-inline")]
        public bool? Inline { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        [HtmlAttributeName("frb-items")]
        public IEnumerable<SelectListItem>? Items { get; set; }
    }
}

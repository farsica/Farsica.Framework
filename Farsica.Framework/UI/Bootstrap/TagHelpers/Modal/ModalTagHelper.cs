namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal")]
    public class ModalTagHelper : TagHelper<ModalTagHelper, ModalTagHelperService>
    {
        public ModalTagHelper(ModalTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-size")]
        public ModalSize Size { get; set; } = ModalSize.Default;

        [HtmlAttributeName("frb-centered")]
        public bool? Centered { get; set; } = false;

        [HtmlAttributeName("frb-static")]
        public bool? Static { get; set; } = false;
    }
}

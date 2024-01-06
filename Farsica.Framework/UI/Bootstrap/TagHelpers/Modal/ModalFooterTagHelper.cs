namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-footer")]
    public class ModalFooterTagHelper(ModalFooterTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ModalFooterTagHelper, ModalFooterTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-buttons")]
        public ModalButtons Buttons { get; set; }

        [HtmlAttributeName("frb-button-alignment")]
        public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.Default;
    }
}

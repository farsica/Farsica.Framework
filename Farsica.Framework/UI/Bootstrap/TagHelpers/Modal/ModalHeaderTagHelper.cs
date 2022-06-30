namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-header")]
    public class ModalHeaderTagHelper : TagHelper<ModalHeaderTagHelper, ModalHeaderTagHelperService>
    {
        public ModalHeaderTagHelper(ModalHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }
    }
}

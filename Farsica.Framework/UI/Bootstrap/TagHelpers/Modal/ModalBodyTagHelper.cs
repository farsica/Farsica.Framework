namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Modal
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-body")]
    public class ModalBodyTagHelper(ModalBodyTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ModalBodyTagHelper, ModalBodyTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-checkbox")]
    public class CheckBoxTagHelper(CheckBoxTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CheckBoxTagHelper, CheckBoxTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-label")]
        public string? Label { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool IsDisabled { get; set; } = false;
    }
}

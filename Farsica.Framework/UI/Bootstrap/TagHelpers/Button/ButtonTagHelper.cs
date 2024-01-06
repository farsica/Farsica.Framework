namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonTagHelper(ButtonTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ButtonTagHelper, ButtonTagHelperService>(tagHelperService, optionsAccessor), IButtonTagHelperBase
    {
        [HtmlAttributeName("frb-button-type")]
        public ButtonType ButtonType { get; set; } = ButtonType.Default;

        [HtmlAttributeName("frb-size")]
        public ButtonSize Size { get; set; } = ButtonSize.Default;

        [HtmlAttributeName("frb-busy-text")]
        public string? BusyText { get; set; }

        [HtmlAttributeName("frb-text")]
        public string? Text { get; set; }

        [HtmlAttributeName("frb-icon")]
        public string? Icon { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        [HtmlAttributeName("frb-icon-type")]
        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;
    }
}

namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("input", Attributes = "frb-button", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("frb-link-button", TagStructure = TagStructure.WithoutEndTag)]
    public class LinkButtonTagHelper : TagHelper<LinkButtonTagHelper, LinkButtonTagHelperService>, IButtonTagHelperBase
    {
        public LinkButtonTagHelper(LinkButtonTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-button")]
        public ButtonType ButtonType { get; set; }

        [HtmlAttributeName("frb-size")]
        public ButtonSize Size { get; set; } = ButtonSize.Default;

        [HtmlAttributeName("frb-text")]
        public string Text { get; set; }

        [HtmlAttributeName("frb-icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        [HtmlAttributeName("frb-icon-type")]
        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;
    }
}

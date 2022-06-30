namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("frb-form", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormTagHelper : TagHelper<FormTagHelper, FormTagHelperService>
    {
        #region Constructors

        public FormTagHelper(FormTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        #endregion

        [HtmlAttributeName("frb-display-submit-button")]
        public bool? DisplaySubmitButton { get; set; } = true;

        [HtmlAttributeName("frb-antiforgery")]
        public bool? Antiforgery { get; set; } = true;

        [HtmlAttributeName("frb-action")]
        public string Action { get; set; }

        [HtmlAttributeName("frb-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("frb-area")]
        public string Area { get; set; }

        [HtmlAttributeName("frb-page")]
        public string Page { get; set; }

        [HtmlAttributeName("frb-page-handler")]
        public string PageHandler { get; set; }

        [HtmlAttributeName("frb-fragment")]
        public string Fragment { get; set; }

        [HtmlAttributeName("frb-route")]
        public string Route { get; set; }

        [HtmlAttributeName("frb-method")]
        public string Method { get; set; } = "post";

        [HtmlAttributeName("frb-all-route-data", DictionaryAttributePrefix = "frb-route-")]
        public IDictionary<string, string> RouteValues { get; set; }
    }
}

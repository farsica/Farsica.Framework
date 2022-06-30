namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-validation-for")]
    [HtmlTargetElement(Attributes = "frb-validation-summary")]
    public class ValidationAttributeTagHelper : TagHelper<ValidationAttributeTagHelper, ValidationAttributeTagHelperService>
    {
        public ValidationAttributeTagHelper(ValidationAttributeTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

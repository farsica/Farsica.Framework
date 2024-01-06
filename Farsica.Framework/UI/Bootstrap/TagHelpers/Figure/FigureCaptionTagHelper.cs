namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Figure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-figure-caption")]
    public class FigureCaptionTagHelper(FigureCaptionTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<FigureCaptionTagHelper, FigureCaptionTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

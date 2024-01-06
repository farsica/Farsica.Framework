namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Figure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-image", ParentTag = "frb-figure")]
    public class FigureImageTagHelper(FigureImageTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<FigureImageTagHelper, FigureImageTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

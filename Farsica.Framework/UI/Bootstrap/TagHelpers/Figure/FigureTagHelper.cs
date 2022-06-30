namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Figure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-figure")]
    public class FigureTagHelper : TagHelper<FigureTagHelper, FigureTagHelperService>
    {
        public FigureTagHelper(FigureTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

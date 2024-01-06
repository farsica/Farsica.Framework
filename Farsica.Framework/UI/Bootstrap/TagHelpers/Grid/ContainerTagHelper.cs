namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Grid
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-container")]
    public class ContainerTagHelper(ContainerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<ContainerTagHelper, ContainerTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

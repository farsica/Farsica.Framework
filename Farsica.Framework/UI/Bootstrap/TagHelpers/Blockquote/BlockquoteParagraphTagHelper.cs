namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("p", ParentTag = "frb-blockquote")]
    public class BlockquoteParagraphTagHelper(BlockquoteParagraphTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<BlockquoteParagraphTagHelper, BlockquoteParagraphTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

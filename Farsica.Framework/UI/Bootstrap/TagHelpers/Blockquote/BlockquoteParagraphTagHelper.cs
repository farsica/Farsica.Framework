namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("p", ParentTag = "frb-blockquote")]
    public class BlockquoteParagraphTagHelper : TagHelper<BlockquoteParagraphTagHelper, BlockquoteParagraphTagHelperService>
    {
        public BlockquoteParagraphTagHelper(BlockquoteParagraphTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

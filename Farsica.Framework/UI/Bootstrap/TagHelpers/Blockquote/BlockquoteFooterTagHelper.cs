namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-footer", ParentTag = "frb-blockquote")]
    public class BlockquoteFooterTagHelper : TagHelper<BlockquoteFooterTagHelper, BlockquoteFooterTagHelperService>
    {
        public BlockquoteFooterTagHelper(BlockquoteFooterTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

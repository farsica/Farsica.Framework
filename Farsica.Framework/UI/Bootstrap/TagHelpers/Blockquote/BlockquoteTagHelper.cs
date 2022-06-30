namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-blockquote", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BlockquoteTagHelper : TagHelper<BlockquoteTagHelper, BlockquoteTagHelperService>
    {
        public BlockquoteTagHelper(BlockquoteTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}

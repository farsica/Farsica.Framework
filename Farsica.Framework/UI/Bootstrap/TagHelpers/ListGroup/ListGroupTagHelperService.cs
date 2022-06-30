namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ListGroupTagHelperService : TagHelperService<ListGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.AddClass("list-group");

            if (TagHelper.Flush ?? false)
            {
                output.Attributes.AddClass("list-group-flush");
            }
        }
    }
}
namespace Farsica.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ListGroupItemTagHelperService : TagHelperService<ListGroupItemTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            MakeLinkIfHrefIsSet();
            SetTagNameAndAttributes(context, output);
        }

        protected virtual void SetTagNameAndAttributes(TagHelperContext context, TagHelperOutput output)
        {
            SetCommonTagNameAndAttributes(context, output);

            if (TagHelper.TagType == ListItemTagType.Default)
            {
                output.TagName = "li";
            }
            else if (TagHelper.TagType == ListItemTagType.Link)
            {
                output.TagName = "a";
                output.Attributes.AddClass("list-group-item-action");
                output.Attributes.Add("href", TagHelper.Href);
            }
            else if (TagHelper.TagType == ListItemTagType.Button)
            {
                output.TagName = "button";
                output.Attributes.AddClass("list-group-item-action");
            }
        }

        protected virtual void SetCommonTagNameAndAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("list-group-item");

            if (TagHelper.Active ?? false)
            {
                output.Attributes.AddClass("active");
            }

            if (TagHelper.Disabled ?? false)
            {
                output.Attributes.AddClass("disabled");
            }

            if (TagHelper.Type != ListItemType.Default)
            {
                output.Attributes.AddClass("list-group-item-" + TagHelper.Type.ToString().ToLowerInvariant());
            }
        }

        protected virtual void MakeLinkIfHrefIsSet()
        {
            if (!string.IsNullOrWhiteSpace(TagHelper.Href))
            {
                TagHelper.TagType = ListItemTagType.Link;
            }
        }
    }
}
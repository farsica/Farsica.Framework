﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class CollapseButtonTagHelperService : TagHelperService<CollapseButtonTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddCommonAttributes(context, output);

            if (output.TagName is "frb-button" or "button")
            {
                AddButtonAttributes(context, output);
            }
            else if (output.TagName == "a")
            {
                AddLinkAttributes(context, output);
            }
        }

        protected virtual void AddCommonAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-toggle", "collapse");
            output.Attributes.Add("aria-expanded", "false");
            output.Attributes.Add("aria-controls", TagHelper.BodyId);
        }

        protected virtual void AddButtonAttributes(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper?.BodyId?.Trim().Split(' ').Length > 1)
            {
                output.Attributes.Add("data-target", ".multi-collapse");
                return;
            }

            output.Attributes.Add("data-target", "#" + TagHelper?.BodyId);
        }

        protected virtual void AddLinkAttributes(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper?.BodyId?.Trim().Split(' ').Length > 1)
            {
                output.Attributes.Add("href", ".multi-collapse");
                return;
            }

            output.Attributes.Add("href", "#" + TagHelper?.BodyId);
        }
    }
}

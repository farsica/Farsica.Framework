﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Alert
{
    using System;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class AlertTagHelperService : TagHelperService<AlertTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            AddClasses(context, output);
            AddDismissButtonIfDismissible(context, output);
        }

        protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("role", "alert");
            output.Attributes.AddClass("alert");

            if (TagHelper.AlertType != AlertType.Default)
            {
                output.Attributes.AddClass("alert-" + TagHelper.AlertType.ToString().ToLowerInvariant());
            }

            if (TagHelper.Dismissible ?? false)
            {
                output.Attributes.AddClass("alert-dismissible");
                output.Attributes.AddClass("fade");
                output.Attributes.AddClass("show");
            }
        }

        protected virtual void AddDismissButtonIfDismissible(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.Dismissible ?? true)
            {
                return;
            }

            var buttonAsHtml =
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" + Environment.NewLine +
                "    <span aria-hidden=\"true\">&times;</span>" + Environment.NewLine +
                " </button>";

            output.PostContent.SetHtmlContent(buttonAsHtml);
        }
    }
}
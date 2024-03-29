﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Popover
{
    using System.Linq;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class PopoverTagHelperService : TagHelperService<PopoverTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.Disabled ?? true)
            {
                SetDataToggle(context, output);
                SetDataPlacement(context, output);
                SetPopoverData(context, output);
                SetDataTriggerIfDismissible(context, output);
                SetDataTriggerIfHoverable(context, output);
            }
            else
            {
                SetDisabled(context, output);
            }
        }

        protected virtual void SetDisabled(TagHelperContext context, TagHelperOutput output)
        {
            var triggerAsHtml = TagHelper.Dismissible ?? false ? "data-trigger=\"focus\" " : string.Empty;
            if (TagHelper.Hoverable ?? false)
            {
                if (triggerAsHtml.Contains("focus"))
                {
                    triggerAsHtml = triggerAsHtml.Replace("focus", "focus hover");
                }
                else
                {
                    triggerAsHtml = "data-trigger=\"hover\" ";
                }
            }

            var dataPlacementAsHtml = "data-placement=\"" + GetDirectory().ToString().ToLowerInvariant() + "\" ";

            // data-placement="default" with data-trigger="focus" causes Cannot read property 'indexOf' of undefined at computeAutoPlacement(bootstrap.bundle.js?_v=637146714627330435:2185) error
            if (IsDismissibleOrHoverable() && GetDirectory() == PopoverDirectory.Default)
            {
                // dataPlacementAsHtml = string.Empty; //bootstrap default placement is right, frb's is top.
                dataPlacementAsHtml = dataPlacementAsHtml.Replace("default", "top");
            }

            var titleAttribute = output.Attributes.FirstOrDefault(at => at.Name == "title");
            var titleAsHtml = titleAttribute is null ? string.Empty : "title=\"" + titleAttribute.Value + "\" ";
            var preElementHtml = "<span tabindex=\"0\" class=\"d-inline-block\" " + titleAsHtml + triggerAsHtml + dataPlacementAsHtml + "data-toggle=\"popover\" data-content=\"" + GetDataContent() + "\">";
            var postElementHtml = "</span>";

            output.PreElement.SetHtmlContent(preElementHtml);
            output.PostElement.SetHtmlContent(postElementHtml);

            output.Attributes.Add("style", "pointer-events: none;");
        }

        protected virtual void SetDataTriggerIfDismissible(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper?.Dismissible ?? false)
            {
                output.Attributes.Add("data-trigger", "focus");
            }
        }

        protected virtual void SetDataTriggerIfHoverable(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper?.Hoverable ?? false)
            {
                // If already has focus data trigger
                if (output.Attributes.TryGetAttribute("data-trigger", out _))
                {
                    output.Attributes.SetAttribute(new TagHelperAttribute("data-trigger", "focus hover"));
                }
                else
                {
                    output.Attributes.Add("data-trigger", "hover");
                }
            }
        }

        protected virtual void SetDataToggle(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-toggle", "popover");
        }

        protected virtual void SetDataPlacement(TagHelperContext context, TagHelperOutput output)
        {
            var directory = GetDirectory();
            if (directory == PopoverDirectory.Default)
            {
                directory = PopoverDirectory.Bottom;
            }

            output.Attributes.Add("data-placement", directory.ToString().ToLowerInvariant());
        }

        protected virtual void SetPopoverData(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-content", GetDataContent());
        }

        protected virtual string? GetDataContent()
        {
            switch (GetDirectory())
            {
                case PopoverDirectory.Top:
                    return TagHelper?.PopoverTop;
                case PopoverDirectory.Right:
                    return TagHelper?.PopoverRight;
                case PopoverDirectory.Bottom:
                    return TagHelper?.PopoverBottom;
                case PopoverDirectory.Left:
                    return TagHelper?.PopoverLeft;
                default:
                    return TagHelper?.Popover;
            }
        }

        protected virtual PopoverDirectory GetDirectory()
        {
            if (!string.IsNullOrEmpty(TagHelper?.PopoverTop))
            {
                return PopoverDirectory.Top;
            }

            if (!string.IsNullOrEmpty(TagHelper?.PopoverBottom))
            {
                return PopoverDirectory.Bottom;
            }

            if (!string.IsNullOrEmpty(TagHelper?.PopoverRight))
            {
                return PopoverDirectory.Right;
            }

            if (!string.IsNullOrEmpty(TagHelper?.PopoverLeft))
            {
                return PopoverDirectory.Left;
            }

            return PopoverDirectory.Default;
        }

        protected virtual bool IsDismissibleOrHoverable()
        {
            if (TagHelper?.Dismissible ?? false)
            {
                return true;
            }

            if (TagHelper?.Hoverable ?? false)
            {
                return true;
            }

            return false;
        }
    }
}

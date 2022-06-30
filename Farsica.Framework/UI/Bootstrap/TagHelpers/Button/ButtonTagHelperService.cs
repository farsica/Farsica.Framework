namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Farsica.Framework.Resources;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class ButtonTagHelperService : ButtonTagHelperServiceBase<ButtonTagHelper>
    {
        protected const string DataBusyTextAttributeName = "data-busy-text";

        public ButtonTagHelperService()
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            output.TagName = "button";
            AddType(context, output);
            AddBusyText(context, output);
        }

        protected virtual void AddType(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddIfNotExist("type", "button");
        }

        protected virtual void AddBusyText(TagHelperContext context, TagHelperOutput output)
        {
            var busyText = TagHelper.BusyText ?? UIResource.ProcessingWithThreeDot;
            if (string.IsNullOrWhiteSpace(busyText))
            {
                return;
            }

            output.Attributes.SetAttribute(DataBusyTextAttributeName, busyText);
        }
    }
}
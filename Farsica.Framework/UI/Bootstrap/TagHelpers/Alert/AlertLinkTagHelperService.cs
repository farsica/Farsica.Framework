namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Alert
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class AlertLinkTagHelperService : TagHelperService<AlertLinkTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("alert-link");
            output.Attributes.RemoveAll("frb-alert-link");
        }
    }
}
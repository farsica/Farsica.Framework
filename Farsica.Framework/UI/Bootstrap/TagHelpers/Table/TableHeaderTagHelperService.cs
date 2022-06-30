namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Table
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class TableHeaderTagHelperService : TagHelperService<TableHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetTheme(context, output);
        }

        protected virtual void SetTheme(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Theme)
            {
                case TableHeaderTheme.Default:
                    return;
                case TableHeaderTheme.Dark:
                    output.Attributes.AddClass("thead-dark");
                    return;
                case TableHeaderTheme.Light:
                    output.Attributes.AddClass("thead-light");
                    return;
            }
        }
    }
}
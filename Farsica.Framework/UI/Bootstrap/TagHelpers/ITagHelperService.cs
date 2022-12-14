namespace Farsica.Framework.UI.Bootstrap.TagHelpers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public interface ITagHelperService<TTagHelper>
        where TTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        TTagHelper? TagHelper { get; }

        int Order { get; }

        void Init(TagHelperContext context);

        void Process(TagHelperContext context, TagHelperOutput output);

        Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
    }
}

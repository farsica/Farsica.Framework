namespace Farsica.Framework.UI.Bootstrap.TagHelpers
{
    using System.Diagnostics.CodeAnalysis;
    using Farsica.Framework.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    public abstract class TagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private readonly IOptions<MvcViewOptions> optionsAccessor;

        private string? elementName;

        private string? elementId;

        protected TagHelper(IOptions<MvcViewOptions> optionsAccessor)
        {
            this.optionsAccessor = optionsAccessor;
        }

        [NotNull]
        [HtmlAttributeName("frb-for")]
        public virtual ModelExpression? For { get; set; }

        [HtmlAttributeName("frb-name")]
        public virtual string? Name { get; set; }

        [HtmlAttributeName("frb-value")]
        public virtual object? Value { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        protected string? ElementName => elementName ??= NameAndIdProvider.GetFullHtmlFieldName(ViewContext, Name ?? For?.Name);

        protected string? ElementId => elementId ??= NameAndIdProvider.CreateSanitizedId(ViewContext, ElementName, optionsAccessor.Value.HtmlHelperOptions.IdAttributeDotReplacement);
    }
}

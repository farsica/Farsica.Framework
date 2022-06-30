namespace Farsica.Framework.UI.Bootstrap.TagHelpers
{
    using System.Threading.Tasks;
    using Farsica.Framework.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    public abstract class TagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private readonly IOptions<MvcViewOptions> optionsAccessor;

        private string elementName;

        private string elementId;

        protected TagHelper(IOptions<MvcViewOptions> optionsAccessor)
        {
            this.optionsAccessor = optionsAccessor;
        }

        [HtmlAttributeName("frb-for")]
        public virtual ModelExpression For { get; set; }

        [HtmlAttributeName("frb-name")]
        public virtual string Name { get; set; }

        [HtmlAttributeName("frb-value")]
        public virtual object Value { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected string ElementName => elementName ??= NameAndIdProvider.GetFullHtmlFieldName(ViewContext, Name ?? For.Name);

        protected string ElementId => elementId ??= NameAndIdProvider.CreateSanitizedId(ViewContext, ElementName, optionsAccessor.Value.HtmlHelperOptions.IdAttributeDotReplacement);
    }

    public abstract class TagHelper<TTagHelper, TService> : TagHelper
        where TTagHelper : TagHelper<TTagHelper, TService>
        where TService : class, ITagHelperService<TTagHelper>
    {
        protected TagHelper(TService service, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            Service = service;
            (Service as TagHelperService<TTagHelper>).TagHelper = (TTagHelper)this;
        }

        public override int Order => Service.Order;

        public virtual bool DisplayRequiredSymbol { get; set; } = true;

        protected TService Service { get; }

        public override void Init(TagHelperContext context)
        {
            Service.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Service.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Service.ProcessAsync(context, output);
        }
    }
}

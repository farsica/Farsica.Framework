namespace Farsica.Framework.UI.Bootstrap.TagHelpers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    public abstract class TagHelper<TTagHelper, TService> : TagHelper
        where TTagHelper : TagHelper<TTagHelper, TService>
        where TService : class, ITagHelperService<TTagHelper>
    {
        protected TagHelper(TService service, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            Service = service;
            var helper = Service as TagHelperService<TTagHelper>;
            if (helper is not null)
            {
                helper.TagHelper = (TTagHelper)this;
            }
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

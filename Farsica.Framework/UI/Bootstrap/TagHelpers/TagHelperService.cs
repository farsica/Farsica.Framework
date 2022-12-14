namespace Farsica.Framework.UI.Bootstrap.TagHelpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    public abstract class TagHelperService<TTagHelper> : ITagHelperService<TTagHelper>
        where TTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        protected const string FormGroupContents = "FormGroupContents";
        protected const string TabItems = "TabItems";
        protected const string AccordionItems = "AccordionItems";
        protected const string BreadcrumbItemsContent = "BreadcrumbItemsContent";
        protected const string CarouselItemsContent = "CarouselItemsContent";
        protected const string TabItemsDataTogglePlaceHolder = "{_data_toggle_Placeholder_}";
        protected const string TabItemNamePlaceHolder = "{_Tab_Tag_Name_Placeholder_}";
        protected const string FormContentPlaceHolder = "{_FormContentPlaceHolder_}";
        protected const string TabItemActivePlaceholder = "{_Tab_Active_Placeholder_}";
        protected const string TabDropdownItemsActivePlaceholder = "{_Tab_DropDown_Items_Placeholder_}";
        protected const string TabItemShowActivePlaceholder = "{_Tab_Show_Active_Placeholder_}";
        protected const string BreadcrumbItemActivePlaceholder = "{_Breadcrumb_Active_Placeholder_}";
        protected const string CarouselItemActivePlaceholder = "{_CarouselItem_Active_Placeholder_}";
        protected const string TabItemSelectedPlaceholder = "{_Tab_Selected_Placeholder_}";
        protected const string AccordionParentIdPlaceholder = "{_Parent_Accordion_Id_}";

        [NotNull]
        public TTagHelper? TagHelper { get; internal set; }

        public virtual int Order { get; }

        public virtual void Init(TagHelperContext context)
        {
        }

        public virtual void Process(TagHelperContext context, TagHelperOutput output)
        {
        }

        public virtual Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            return Task.CompletedTask;
        }
    }
}

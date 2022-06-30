namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Button
{
    public interface IButtonTagHelperBase
    {
        ButtonType ButtonType { get; }

        ButtonSize Size { get; }

        string Text { get; }

        string Icon { get; }

        bool? Disabled { get; }

        FontIconType IconType { get; }
    }
}
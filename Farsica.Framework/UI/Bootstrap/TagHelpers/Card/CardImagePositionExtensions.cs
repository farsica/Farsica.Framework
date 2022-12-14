namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using System;

    public static class CardImagePositionExtensions
    {
        public static string? ToClassName(this CardImagePosition position)
        {
            switch (position)
            {
                case CardImagePosition.None:
                    return "card-img";
                case CardImagePosition.Top:
                    return "card-img-top";
                case CardImagePosition.Bottom:
                    return "card-img-bottom";
                default:
                    throw new ArgumentOutOfRangeException("Unknown CardImagePosition: " + position);
            }
        }
    }
}
﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-title")]
    public class CardTitleTagHelper(CardTitleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardTitleTagHelper, CardTitleTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-default-heading")]
        public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H5;

        [HtmlAttributeName("frb-heading")]
        public HtmlHeadingType Heading { get; set; } = DefaultHeading;
    }
}

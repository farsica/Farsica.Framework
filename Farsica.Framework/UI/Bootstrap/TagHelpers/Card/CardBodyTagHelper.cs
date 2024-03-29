﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-body")]
    public class CardBodyTagHelper(CardBodyTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<CardBodyTagHelper, CardBodyTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-title")]
        public string? Title { get; set; }

        [HtmlAttributeName("frb-subtitle")]
        public string? Subtitle { get; set; }
    }
}

﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Nav
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav")]
    public class NavTagHelper(NavTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<NavTagHelper, NavTagHelperService>(tagHelperService, optionsAccessor)
    {
        [HtmlAttributeName("frb-align")]
        public NavAlign Align { get; set; } = NavAlign.Default;

        [HtmlAttributeName("frb-nam-style")]
        public NavStyle NavStyle { get; set; } = NavStyle.Default;
    }
}

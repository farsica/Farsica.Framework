﻿namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Form
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-form-content", TagStructure = TagStructure.WithoutEndTag)]
    public class FormContentTagHelper(FormContentTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor) : TagHelper<FormContentTagHelper, FormContentTagHelperService>(tagHelperService, optionsAccessor)
    {
    }
}

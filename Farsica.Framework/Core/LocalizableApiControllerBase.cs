namespace Farsica.Framework.Core
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("{culture=fa}/api/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}")]
    public abstract class LocalizableApiControllerBase<TClass> : ApiControllerBase<TClass>
        where TClass : class
    {
        protected LocalizableApiControllerBase(Lazy<ILogger<TClass>> logger, Lazy<IStringLocalizer<TClass>> localizer)
            : base(logger)
        {
            Localizer = localizer;
        }

        protected Lazy<IStringLocalizer<TClass>> Localizer { get; }
    }
}

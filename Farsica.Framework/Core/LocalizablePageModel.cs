namespace Farsica.Framework.Core
{
    using System;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class LocalizablePageModel<T> : PageModel<T>
        where T : class
    {
        protected LocalizablePageModel(Lazy<ILogger<T>> logger, Lazy<IStringLocalizer<T>> localizer)
            : base(logger)
        {
            Localizer = localizer;
        }

        protected Lazy<IStringLocalizer<T>> Localizer { get; }
    }
}

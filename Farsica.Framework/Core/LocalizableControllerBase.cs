namespace Farsica.Framework.Core
{
    using System;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class LocalizableControllerBase<TClass> : ControllerBase<TClass>
        where TClass : class
    {
        protected LocalizableControllerBase(Lazy<ILogger<TClass>> logger, Lazy<IStringLocalizer<TClass>> localizer)
            : base(logger)
        {
            Localizer = localizer;
        }

        protected Lazy<IStringLocalizer<TClass>> Localizer { get; }
    }
}

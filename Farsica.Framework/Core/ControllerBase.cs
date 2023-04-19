namespace Farsica.Framework.Core
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class ControllerBase<TClass> : Controller
        where TClass : class
    {
        protected ControllerBase(Lazy<ILogger<TClass>> logger)
        {
            Logger = logger;
        }

        protected Lazy<ILogger<TClass>> Logger { get; }

        [NonAction]
        public RedirectToPageResult RedirectToAreaPage(string? pageName, string? area, object? routeValues = null)
            => RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix) ?? string.Empty, Globals.PrepareValues(routeValues, area));

        [NonAction]
        public RedirectToActionResult RedirectToAreaAction(string? actionName, string? controllerName, string? area, object? routeValues = null)
            => RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public override RedirectToActionResult RedirectToAction(string? actionName, string? controllerName, object? routeValues, string? fragment)
            => base.RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        [NonAction]
        public override RedirectToPageResult RedirectToPage(string? pageName, string? pageHandler, object? routeValues, string? fragment)
            => base.RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix) ?? string.Empty, pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}

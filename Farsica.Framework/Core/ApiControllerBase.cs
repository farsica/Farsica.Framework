namespace Farsica.Framework.Core
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [ApiController]

    // [Route("api/[[area]]/[[controller]]/[[action]]")]
    [Route("{culture=fa}/api/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}")]
    public abstract class ApiControllerBase<TClass, TUser> : ControllerBase
        where TClass : class
        where TUser : class
    {
        protected ApiControllerBase(Lazy<UserManager<TUser>> userManager, Lazy<ILogger<TClass>> logger, Lazy<IStringLocalizer<TClass>> localizer)
        {
            Logger = logger;
            UserManager = userManager;
            Localizer = localizer;
        }

        protected Lazy<ILogger<TClass>> Logger { get; }

        protected Lazy<UserManager<TUser>> UserManager { get; }

        protected Lazy<IStringLocalizer<TClass>> Localizer { get; }

        [NonAction]
        public RedirectToPageResult RedirectToAreaPage(string pageName, string area, object routeValues = null)
            => RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public RedirectToActionResult RedirectToAreaAction(string actionName, string controllerName, string area, object routeValues = null)
            => RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public override RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
            => base.RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        [NonAction]
        public override RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
            => base.RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}

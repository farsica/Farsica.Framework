namespace Farsica.Framework.Core
{
    using System;
    using Farsica.Framework.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}")]
    public abstract class ApiControllerBase<TClass> : ControllerBase
        where TClass : class
    {
        protected ApiControllerBase(Lazy<ILogger<TClass>> logger)
        {
            Logger = logger;
        }

        protected Lazy<ILogger<TClass>> Logger { get; }

        [NonAction]
        public RedirectToPageResult RedirectToAreaPage(string pageName, string? area, object? routeValues = null)
            => RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public RedirectToActionResult RedirectToAreaAction(string? actionName, string? controllerName, string? area, object? routeValues = null)
            => RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public override RedirectToActionResult RedirectToAction(string? actionName, string? controllerName, object? routeValues, string? fragment)
            => base.RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        [NonAction]
        public override RedirectToPageResult RedirectToPage(string pageName, string? pageHandler, object? routeValues, string? fragment)
            => base.RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), pageHandler, Globals.PrepareValues(routeValues), fragment);

        public BadRequestObjectResult BadRequest<T>(ApiResponse<T> response)
        {
            return base.BadRequest(response);
        }

        public OkObjectResult Ok<T>(ApiResponse<T> response)
        {
            return base.Ok(response);
        }

        public override UnauthorizedResult Unauthorized()
        {
            return base.Unauthorized();
        }

        public override ForbidResult Forbid()
        {
            return base.Forbid();
        }

        public ObjectResult InternalServerError<T>(ApiResponse<T> response) => new(response)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}

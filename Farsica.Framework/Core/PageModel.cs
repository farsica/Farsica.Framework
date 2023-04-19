namespace Farsica.Framework.Core
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class PageModel<TClass> : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
        where TClass : class
    {
        protected PageModel(Lazy<ILogger<TClass>> logger)
        {
            Logger = logger;
        }

        protected Lazy<ILogger<TClass>> Logger { get; }

        public override BadRequestObjectResult BadRequest(object error)
        {
            return base.BadRequest(error);
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            return base.NotFound(value);
        }

        public override ForbidResult Forbid()
        {
            return base.Forbid();
        }

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            return base.StatusCode(statusCode, value);
        }

        public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request?.Headers["X-Requested-With"].FirstOrDefault() == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(t => t.Errors.Select(e => e.ErrorMessage));
                    context.Result = new BadRequestObjectResult(errors);
                }
                else
                {
                    context.Result = Page();
                }
            }

            // else
            // {
            //    pagingDto = context.HandlerArguments.Any(t => t.Value?.GetType() == typeof(PagingDto)) ? context.HandlerArguments.First(t => t.Value?.GetType() == typeof(PagingDto)).Value as PagingDto : null;
            //    search = context.HandlerArguments.Any(t => t.Value?.GetType() == typeof(ISearch)) ? context.HandlerArguments.First(t => t.Value?.GetType() == typeof(ISearch)).Value as ISearch : null;
            // }
        }

        // public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        // {
        //    base.OnPageHandlerExecuted(context);

        // if (pagingDto?.Export is true)
        //    {
        //        if ((context.Result as ObjectResult)?.Value is GridDataSource gridDataSource)
        //        {
        //            var fileName = context.ActionDescriptor.DisplayName.Split('/', System.StringSplitOptions.RemoveEmptyEntries).Last();
        //            context.Result = genericFactory.GetProvider(pagingDto.ExportType, false).Export(gridDataSource, search, fileName);
        //        }
        //    }
        // }
        public RedirectToPageResult RedirectToAreaPage(string? pageName, string? area, object? routeValues = null)
            => RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area));

        public RedirectToActionResult RedirectToAreaAction(string? actionName, string? controllerName, string? area, object? routeValues = null)
            => RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        public override RedirectToActionResult RedirectToAction(string? actionName, string? controllerName, object? routeValues, string? fragment)
            => base.RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        public override RedirectToPageResult RedirectToPage(string? pageName, string? pageHandler, object? routeValues, string? fragment)
            => base.RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix), pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}

namespace Farsica.Framework.Mvc.Filters
{
    using System.Linq;

    using Farsica.Framework.Core.Utils.Export;
    using Farsica.Framework.Data;
    using Farsica.Framework.Service.Factory;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ExportableActionFilter(IGenericFactory<ExportBase, ExportType> genericFactory) : IActionFilter
    {
        private readonly IGenericFactory<ExportBase, ExportType> genericFactory = genericFactory;
        private PagingDto pagingDto;
        private ISearch search;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            pagingDto = context.ActionArguments.Any(t => t.Value?.GetType() == typeof(PagingDto)) ? context.ActionArguments.First(t => t.Value?.GetType() == typeof(PagingDto)).Value as PagingDto : null;
            search = context.ActionArguments.Any(t => t.Value?.GetType() == typeof(ISearch)) ? context.ActionArguments.First(t => t.Value?.GetType() == typeof(ISearch)).Value as ISearch : null;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (pagingDto?.PageFilter?.ExportType is not null)
            {
                if ((context.Result as ObjectResult)?.Value is GridDataSource gridDataSource)
                {
                    var fileName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;
                    context.Result = genericFactory.GetProvider(pagingDto.PageFilter.ExportType, false).Export(gridDataSource, search, fileName);
                }
            }
        }
    }
}

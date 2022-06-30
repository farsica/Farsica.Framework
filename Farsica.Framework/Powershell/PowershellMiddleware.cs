namespace Farsica.Framework.Powershell
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class PowershellMiddleware
    {
        private readonly RequestDelegate next;
        private readonly RouteCollection routes;

        public PowershellMiddleware(RequestDelegate next, RouteCollection routes)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.routes = routes ?? throw new ArgumentNullException(nameof(routes));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var context = new PowershellContext(httpContext);
            var findResult = routes.FindDispatcher(httpContext.Request.Path.Value);

            if (findResult == null)
            {
                await next.Invoke(httpContext);
                return;
            }

            // foreach (var filter in _options.Authorization)
            // {
            //    if (!filter.Authorize(context))
            //    {
            //        var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated;

            // httpContext.Response.StatusCode = isAuthenticated == true
            //            ? (int)HttpStatusCode.Forbidden
            //            : (int)HttpStatusCode.Unauthorized;

            // return;
            //    }
            // }
            var antiforgery = httpContext.RequestServices.GetService<IAntiforgery>();
            if (antiforgery != null)
            {
                var requestValid = await antiforgery.IsRequestValidAsync(httpContext);

                if (!requestValid)
                {
                    // Invalid or missing CSRF token
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            context.UriMatch = findResult.Item2;

            await findResult.Item1.Dispatch(context);
        }
    }
}

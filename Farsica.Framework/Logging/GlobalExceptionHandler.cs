namespace Farsica.Framework.Logging
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogException(exception);

            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            await httpContext.Response.WriteAsJsonAsync(
                new ApiResponse<object>
                {
                    Errors = [new Error { Message = exception.Message, }],
                }, cancellationToken);

            return true;
        }
    }
}

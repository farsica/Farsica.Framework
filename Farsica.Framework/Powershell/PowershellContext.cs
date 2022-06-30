namespace Farsica.Framework.Powershell
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class PowershellContext
    {
        public PowershellContext(HttpContext httpContext)
        {
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Request = new PowershellRequest(httpContext);
            Response = new PowershellResponse(httpContext);

            var antiforgery = HttpContext.RequestServices.GetService<IAntiforgery>();
            var tokenSet = antiforgery?.GetAndStoreTokens(HttpContext);

            if (tokenSet != null)
            {
                AntiforgeryHeader = tokenSet.HeaderName;
                AntiforgeryToken = tokenSet.RequestToken;
            }
        }

        public Match UriMatch { get; set; }

        public PowershellRequest Request { get; }

        public PowershellResponse Response { get; }

        public string AntiforgeryHeader { get; set; }

        public string AntiforgeryToken { get; set; }

        public HttpContext HttpContext { get; }
    }
}

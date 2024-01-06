namespace Farsica.Framework.Localization
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;

    public class RouteValueRequestCultureProvider(CultureInfo[] cultures) : IRequestCultureProvider
    {
        private readonly CultureInfo[] cultures = cultures;

        public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;

            if (string.IsNullOrEmpty(path))
            {
                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(Constants.DefaultLanguageCode));
            }

            var routeValues = httpContext.Request.Path.Value.Split('/');
            if (routeValues.Length <= 1)
            {
                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(Constants.DefaultLanguageCode));
            }

            if (!cultures.Exists(t => t.Name.Equals(routeValues[1], StringComparison.InvariantCultureIgnoreCase)))
            {
                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(Constants.DefaultLanguageCode));
            }

            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(routeValues[1]));
        }
    }
}

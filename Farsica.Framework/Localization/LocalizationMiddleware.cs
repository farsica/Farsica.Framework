namespace Farsica.Framework.Localization
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class LocalizationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate next = next;

        public Task Invoke(HttpContext httpContext)
        {
            var culture = httpContext.GetRouteData()?.Values[Constants.LanguageIdentifier] as string;
            if (!culture.IsNullOrEmpty() && CultureExtensions.GetAtomicValues().Contains(culture))
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = Globals.GetCulture(culture);
            }

            // if(httpContext.Request.Cookies.ContainsKey(Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName))
            return next(httpContext);
        }
    }
}

namespace Farsica.Framework.Localization
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(Core.Constants.LanguageIdentifier))
            {
                return false;
            }

            var lang = values[Core.Constants.LanguageIdentifier].ToString();

            return CultureExtensions.GetAtomicValues().Any(t => t.Equals(lang, System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

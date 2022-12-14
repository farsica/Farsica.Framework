namespace Farsica.Framework.Mvc
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Farsica.Framework.Core;

    using Microsoft.AspNetCore.Mvc;

    public static class UrlHelperExtensions
    {
        public static string? Action(this IUrlHelper urlHelper, string? action, string? controller, string? area, object values)
            => urlHelper.Action(action, controller?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(values, area));

        public static string? Page(this IUrlHelper urlHelper, string? pageName, string? area, object values)
            => urlHelper.Page(pageName?.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(values, area));

        public static string? LocalizedAction(this IUrlHelper helper, string? action = null, string? controller = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null)
            => Prepare(helper.Action(action, controller?.TrimEnd(Constants.ControllerPostfix), values, protocol, host, fragment));

        public static string? LocalizedRouteUrl(this IUrlHelper helper, object values)
            => helper.LocalizedRouteUrl(null, values);

        public static string? LocalizedRouteUrl(this IUrlHelper helper, string? routeName, object? values = null, string? protocol = null, string? host = null, string? fragment = null)
            => Prepare(helper.RouteUrl(routeName, values, protocol, host, fragment));

        public static string? LocalizedPage(this IUrlHelper urlHelper, string? pageName, string? pageHandler = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null)
            => Prepare(urlHelper.Page(pageName?.TrimEnd(Constants.PagePostfix), pageHandler, values, protocol, host, fragment));

        public static string? LocalizedActionLink(this IUrlHelper helper, string? action = null, string? controller = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null)
            => Prepare(helper.ActionLink(action, controller?.TrimEnd(Constants.ControllerPostfix), values, protocol, host, fragment));

        public static string? LocalizedPageLink(this IUrlHelper helper, string? pageName = null, string? pageHandler = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null)
            => Prepare(helper.PageLink(pageName?.TrimEnd(Constants.PagePostfix), pageHandler, values, protocol, host, fragment));

        private static string? Prepare(string? url)
        {
            var sb = new StringBuilder("/");
            if (url == "/")
            {
                return sb.Append(CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Append('/').ToString();
            }

            var lst = url?.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (lst is null)
            {
                return null;
            }

            if (Localization.CultureExtensions.GetAtomicValues().Any(t => t.Equals(lst[0], StringComparison.InvariantCultureIgnoreCase)))
            {
                lst[0] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
            else
            {
                sb.Append(CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Append('/');
            }

            return sb.Append(string.Join("/", lst)).ToString();
        }
    }
}

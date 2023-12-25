namespace Farsica.Framework.Startup
{
    using Microsoft.Extensions.Configuration;

    public abstract class Startup : Startup<Startup, Startup>
    {
        protected Startup(IConfiguration configuration, string? defaultNamespace = null, bool localization = true, bool authentication = true, bool razorPages = true, bool antiforgery = true, bool https = true, bool views = true)
            : base(configuration, defaultNamespace, localization, authentication, razorPages, antiforgery, https, views, false)
        {
        }
    }
}

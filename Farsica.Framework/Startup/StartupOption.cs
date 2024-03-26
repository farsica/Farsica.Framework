namespace Farsica.Framework.Startup
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;

    public class StartupOption
    {
#pragma warning disable SA1206 // Declaration keywords should follow order
        public required IConfiguration Configuration { get; set; }
#pragma warning restore SA1206 // Declaration keywords should follow order

        public string? DefaultNamespace { get; set; } = "Farsica";

        public bool Localization { get; set; }

        public bool Authentication { get; set; }

        public bool RazorPages { get; set; }

        public bool RazorViews { get; set; }

        public bool Antiforgery { get; set; }

        public bool Https { get; set; }

        public bool Identity { get; set; }

        public Func<IServiceProvider, DelegatingHandler>? HttpClientMessageHandler { get; set; }

        public string? ErrorCodePrefix { get; set; }
    }
}

namespace Farsica.Framework.Powershell
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public sealed class PowershellResponse
    {
        private readonly HttpContext context;

        public PowershellResponse(HttpContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string ContentType
        {
            get { return context.Response.ContentType; }
            set { context.Response.ContentType = value; }
        }

        public int StatusCode
        {
            get { return context.Response.StatusCode; }
            set { context.Response.StatusCode = value; }
        }

        public Stream Body => context.Response.Body;

        public Task WriteAsync(string text) => context.Response.WriteAsync(text);

        public void SetExpire(DateTimeOffset? value) => context.Response.Headers["Expires"] = value?.ToString("r", CultureInfo.InvariantCulture);
    }
}

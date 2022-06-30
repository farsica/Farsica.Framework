namespace Farsica.Framework.Powershell
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public sealed class PowershellRequest
    {
        private readonly HttpContext context;

        public PowershellRequest(HttpContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string Method => context.Request.Method;

        public string Path => context.Request.Path.Value;

        public string PathBase => context.Request.PathBase.Value;

        public string LocalIpAddress => context.Connection.LocalIpAddress.ToString();

        public string RemoteIpAddress => context.Connection.RemoteIpAddress.ToString();

        public string GetQuery(string key) => context.Request.Query[key];

        public async Task<IList<string>> GetFormValuesAsync(string key)
        {
            var form = await context.Request.ReadFormAsync();
            return form[key];
        }
    }
}

namespace Farsica.Framework.Converter
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class ConfigureJsonOptions(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) : IConfigureOptions<JsonOptions>
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(new ServiceProviderDummyConverter(httpContextAccessor, serviceProvider));
            options.JsonSerializerOptions.TypeInfoResolver = new CustomtJsonTypeInfoResolver();
        }
    }
}

namespace Farsica.Framework.Identity.DataProtection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Farsica.Framework.DataAccess.Audit;
    using Microsoft.AspNetCore.DataProtection.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public class CustomXmlRepository : IXmlRepository
    {
        private readonly IServiceProvider services;

        public CustomXmlRepository(IServiceProvider services)
        {
            this.services = services;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            using var scope = services.CreateScope();
            var dataProtectionService = scope.ServiceProvider.GetRequiredService<IDataProtectionService>();
            return dataProtectionService.GetDataProtections()?.ToArray() ?? [];
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            using var scope = services.CreateScope();
            var dataProtectionService = scope.ServiceProvider.GetRequiredService<IDataProtectionService>();
            dataProtectionService.AddDataProtection(new DataProtectionKey
            {
                FriendlyName = friendlyName,
                Xml = element.ToString(SaveOptions.DisableFormatting),
            });
        }
    }
}

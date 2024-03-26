namespace Farsica.Framework.Identity.DataProtection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Farsica.Framework.Core;
    using Farsica.Framework.DataAccess.Audit;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Farsica.Framework.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using NUlid;

    public class DataProtectionService : ServiceBase<DataProtectionService>, IDataProtectionService
    {
        public DataProtectionService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<ILogger<DataProtectionService>> logger)
        : base(unitOfWorkProvider, httpContextAccessor, logger)
        {
        }

        public IEnumerable<XElement>? GetDataProtections()
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                return uow.GetRepository<DataProtectionKey, Ulid>().GetManyQueryable().Select(t => t.Xml).ToList().Select(XElement.Parse);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return null;
            }
        }

        public void AddDataProtection(DataProtectionKey dataProtectionKey)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<DataProtectionKey, Ulid>();
                repository.Add(dataProtectionKey);
                uow.SaveChanges();
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
            }
        }
    }
}

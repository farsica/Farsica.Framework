namespace Farsica.Framework.Identity.DataProtection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Farsica.Framework.DataAccess.Audit;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using NUlid;

    public class DataProtectionService(IUnitOfWorkProvider unitOfWorkProvider) : IDataProtectionService
    {
        public IEnumerable<XElement>? GetDataProtections()
        {
            var uow = unitOfWorkProvider.CreateUnitOfWork();
            return uow.GetRepository<DataProtectionKey, Ulid>().GetManyQueryable().Select(t => t.Xml).ToList().Select(XElement.Parse);
        }

        public void AddDataProtection(DataProtectionKey dataProtectionKey)
        {
            var uow = unitOfWorkProvider.CreateUnitOfWork();
            var repository = uow.GetRepository<DataProtectionKey, Ulid>();
            repository.Add(dataProtectionKey);
            uow.SaveChanges();
        }
    }
}

namespace Farsica.Framework.DataAccess.Audit
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Identity.DataProtection;

    [Injectable]
    public interface IDataProtectionService
    {
        IEnumerable<XElement>? GetDataProtections();

        void AddDataProtection(DataProtectionKey dataProtectionKey);
    }
}

namespace Farsica.Framework.DataAccess.Audit
{
    using Farsica.Framework.Data.Enumeration;

    public class AuditType : Enumeration<AuditType, byte>
    {
        public static readonly AuditType Added = new(nameof(Added), 0);
        public static readonly AuditType Modified = new(nameof(Modified), 1);
        public static readonly AuditType Deleted = new(nameof(Deleted), 2);

        public AuditType()
        {
        }

        public AuditType(string name, byte value)
            : base(name, value)
        {
        }
    }
}

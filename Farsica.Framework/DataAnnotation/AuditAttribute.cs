namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AuditAttribute : Attribute
    {
        public AuditAttribute(int entityType)
        {
            EntityType = entityType;
        }

        public int EntityType { get; }
    }
}

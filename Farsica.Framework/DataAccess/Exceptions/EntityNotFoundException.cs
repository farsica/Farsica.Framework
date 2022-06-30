namespace Farsica.Framework.DataAccess.Exceptions
{
    using System;

#pragma warning disable CA1032 // Implement standard exception constructors
    public class EntityNotFoundException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public EntityNotFoundException(string entityName, int entityKey)
        {
            EntityName = entityName;
            EntityKey = entityKey;
            Message = $"Entity of type '{entityName}' and key {EntityKey} not found in the current context.";
        }

        public string EntityName { get; set; }

        public int EntityKey { get; set; }

        public override string Message { get; }
    }
}

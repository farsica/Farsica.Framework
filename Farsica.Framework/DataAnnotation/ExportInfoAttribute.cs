namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ExportInfoAttribute : Attribute
    {
        public bool Ignore { get; set; }

        public string TrueResourceKey { get; set; }

        public string FalseResourceKey { get; set; }

        public Type ResourceType { get; set; }
    }
}

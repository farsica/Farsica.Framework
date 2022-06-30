namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScaffoldColumnAttribute : System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute
    {
        public ScaffoldColumnAttribute(bool scaffold)
            : base(scaffold)
        {
        }
    }
}

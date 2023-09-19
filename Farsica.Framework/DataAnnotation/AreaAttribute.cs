namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AreaAttribute : Microsoft.AspNetCore.Mvc.AreaAttribute
    {
        public AreaAttribute(string areaName, string? displayName = null)
            : base(areaName)
        {
            AreaName = areaName;
            DisplayName = displayName;
        }

        public string AreaName { get; }

        public string? DisplayName { get; }
    }
}

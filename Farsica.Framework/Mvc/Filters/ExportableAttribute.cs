namespace Farsica.Framework.Mvc.Filters
{
    using Microsoft.AspNetCore.Mvc;

    public class ExportableAttribute : TypeFilterAttribute
    {
        public ExportableAttribute()
            : base(typeof(ExportableActionFilter))
        {
        }
    }
}

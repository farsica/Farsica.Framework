namespace Farsica.Framework.Data
{
    using System.Collections.Generic;

    public abstract class ApiResponseBase
    {
        public IEnumerable<Error>? Errors { get; set; }
    }
}

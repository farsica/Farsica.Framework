namespace Farsica.Framework.Mvc.Routing
{
    using System.Collections.Generic;
    using Farsica.Framework.DataAnnotation;

    [Injectable]
    public interface IEndpointDataSource
    {
        IEnumerable<Endpoint>? GetEndpoints(IEnumerable<string>? claims = null, IEnumerable<string>? roles = null);
    }
}

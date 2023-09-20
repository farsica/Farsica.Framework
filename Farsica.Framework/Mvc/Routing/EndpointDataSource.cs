namespace Farsica.Framework.Mvc.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public class EndpointDataSource : IEndpointDataSource
    {
        private readonly Lazy<IEnumerable<Microsoft.AspNetCore.Routing.EndpointDataSource>> endpointSources;

        public EndpointDataSource(Lazy<IEnumerable<Microsoft.AspNetCore.Routing.EndpointDataSource>> endpointSources)
        {
            this.endpointSources = endpointSources;
        }

        public IEnumerable<Endpoint>? GetEndpoints()
        {
            return endpointSources.Value.SelectMany(t => t.Endpoints).Select(t =>
            {
                var area = t.Metadata.OfType<AreaAttribute>().FirstOrDefault();
                var controller = t.Metadata.OfType<ControllerActionDescriptor>().LastOrDefault();
                var display = t.Metadata.OfType<DisplayAttribute>();
                return new Endpoint
                {
                    EndpointName = t.DisplayName,
                    Area = area is null ? null : new(area.AreaName, area.DisplayName ?? area.AreaName),
                    Controller = controller is null ? null : new(controller.ControllerName, display.FirstOrDefault()?.Name ?? controller.ControllerName),
                    Action = controller is null ? null : new(controller.ActionName, display.LastOrDefault()?.Name ?? controller.ActionName),
                };
            });
        }
    }
}

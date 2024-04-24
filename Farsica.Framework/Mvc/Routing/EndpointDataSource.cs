﻿namespace Farsica.Framework.Mvc.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Identity;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public class EndpointDataSource(Lazy<IEnumerable<Microsoft.AspNetCore.Routing.EndpointDataSource>> endpointSources) : IEndpointDataSource
    {
        private readonly Lazy<IEnumerable<Microsoft.AspNetCore.Routing.EndpointDataSource>> endpointSources = endpointSources;

        public IEnumerable<Endpoint>? GetEndpoints(IEnumerable<string>? claims = null, IEnumerable<string>? roles = null)
        {
            var lst = endpointSources.Value.SelectMany(t => t.Endpoints);
            if (claims is not null || roles is not null)
            {
                lst = lst.Where(t => claims?.Contains(t.DisplayName) == true
                    || t.Metadata.OfType<PermissionAttribute>().FirstOrDefault()?.Roles?.Any(r => roles?.Contains(r) == true) == true);
            }

            return lst.Select(t =>
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

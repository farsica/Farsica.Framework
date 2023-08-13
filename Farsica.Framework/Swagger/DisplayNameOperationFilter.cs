namespace Farsica.Framework.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DisplayNameOperationFilter : IOperationFilter
    {
        public void Apply([NotNull] OpenApiOperation operation, [NotNull] OperationFilterContext context)
        {
            IEnumerable<object> actionAttributes = context.MethodInfo is null ? Array.Empty<object>() : context.MethodInfo.GetCustomAttributes(true);
            IEnumerable<object> metadataAttributes = context.ApiDescription?.ActionDescriptor?.EndpointMetadata is null ? Array.Empty<object>() : context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var actionAndEndpointAttribtues = actionAttributes.Union(metadataAttributes).Distinct();
            ApplySwaggerOperationAttribute(operation, actionAndEndpointAttribtues);

            if (context.ApiDescription?.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var areaName = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AreaAttribute), true)
                    .Cast<AreaAttribute>().FirstOrDefault();
                operation.Tags = areaName != null
                    ? new List<OpenApiTag> { new OpenApiTag { Name = $"{areaName.RouteValue} - {controllerActionDescriptor.ControllerName}" } }
                    : (IList<OpenApiTag>)new List<OpenApiTag> { new OpenApiTag { Name = controllerActionDescriptor.ControllerName } };
            }
        }

        private static void ApplySwaggerOperationAttribute(OpenApiOperation operation, IEnumerable<object> actionAttributes)
        {
            var displayNameAttribute = actionAttributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayNameAttribute is null)
            {
                return;
            }

            if (displayNameAttribute.DisplayName != null)
            {
                operation.Summary = displayNameAttribute.DisplayName;
            }
        }
    }
}

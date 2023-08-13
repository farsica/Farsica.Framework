namespace Farsica.Framework.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
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

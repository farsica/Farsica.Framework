namespace Farsica.Framework.Swagger
{
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DisplayNameDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags ??= new List<OpenApiTag>();

            var controllerNamesAndAttributes = context.ApiDescriptions
                .Select(t => t.ActionDescriptor as ControllerActionDescriptor)
                .Where(t => t != null)
                .GroupBy(t => t!.ControllerName)
                .Select(t => new KeyValuePair<string, IEnumerable<object>>(t.Key, t.First()!.ControllerTypeInfo.GetCustomAttributes(true)));

            foreach (var entry in controllerNamesAndAttributes)
            {
                ApplySwaggerTagAttribute(swaggerDoc, entry.Key, entry.Value);
            }
        }

        private static void ApplySwaggerTagAttribute(OpenApiDocument swaggerDoc, string controllerName, IEnumerable<object> customAttributes)
        {
            var displayNameAttribute = customAttributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayNameAttribute is null)
            {
                return;
            }

            swaggerDoc.Tags.Add(new OpenApiTag
            {
                Name = controllerName,
                Description = displayNameAttribute.DisplayName,
            });
        }
    }
}

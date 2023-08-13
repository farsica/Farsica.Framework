namespace Farsica.Framework.Swagger
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DisplayNameDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags ??= new List<OpenApiTag>();

            foreach (var item in context.ApiDescriptions)
            {
                if (item.ActionDescriptor is not ControllerActionDescriptor descriptor)
                {
                    continue;
                }

                var displayNameAttribute = descriptor.ControllerTypeInfo.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
                var description = displayNameAttribute is null ? string.Empty : displayNameAttribute.DisplayName;

                var areaAttribute = descriptor.ControllerTypeInfo.GetCustomAttributes<AreaAttribute>().FirstOrDefault();
                var name = areaAttribute is null ? descriptor.ControllerName : $"{areaAttribute.RouteValue} - {descriptor.ControllerName}";

                swaggerDoc.Tags.Add(new OpenApiTag
                {
                    Name = name,
                    Description = description,
                });
            }

            swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(t => t.Name).ToList();
        }
    }
}

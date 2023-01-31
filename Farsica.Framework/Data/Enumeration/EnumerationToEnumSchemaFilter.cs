namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class EnumerationToEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!IsSubclassOf(typeof(Enumeration<>), context.Type))
            {
                return;
            }

            var fields = context.Type.GetFields(BindingFlags.Static | BindingFlags.Public);

            schema.Enum = fields.Select(field => new OpenApiString(field.Name)).Cast<IOpenApiAny>().ToList();
            schema.Type = "string";
            schema.Properties = null;
            schema.AllOf = null;
        }

        private static bool IsSubclassOf(Type? generic, Type? toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}

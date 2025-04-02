namespace Farsica.Framework.Swagger
{
    using System.Linq;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data.Enumeration;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class EnumerationToEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var isEnumeration = Globals.IsSubclassOf(context.Type, typeof(Enumeration<,>));
            if (!isEnumeration && !Globals.IsSubclassOf(context.Type, typeof(FlagsEnumeration<>)))
            {
                return;
            }

            var names = EnumerationExtensions.GetNames(context.Type)!
                            .Select(t => new OpenApiString(t)).ToList<IOpenApiAny>();

            if (isEnumeration)
            {
                schema.Enum = names;
                schema.Type = "string";
                schema.Properties = null;
                schema.AllOf = null;
            }
            else
            {
                schema.Properties = null;
                schema.AllOf = null;
                schema.Type = "array";
                schema.Items = new OpenApiSchema
                {
                    Enum = names,
                    Type = "string",
                    Properties = null,
                    AllOf = null,
                };
            }
        }
    }
}

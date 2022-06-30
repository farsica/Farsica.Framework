namespace Farsica.Framework.Mvc.Routing
{
    using Farsica.Framework.Core;
    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => (value as string).Slugify();
    }
}

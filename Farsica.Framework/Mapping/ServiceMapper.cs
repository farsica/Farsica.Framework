namespace Farsica.Framework.Mapping
{
    using System;

    [DataAnnotation.ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class ServiceMapper : MapsterMapper.ServiceMapper, IMapper
    {
        public ServiceMapper(IServiceProvider serviceProvider, TypeAdapterConfig config)
            : base(serviceProvider, config)
        {
        }
    }
}

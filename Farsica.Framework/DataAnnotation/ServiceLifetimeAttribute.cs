﻿namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Core;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ServiceLifetimeAttribute : Attribute
    {
#pragma warning disable CA1019 // Define accessors for attribute arguments
        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string? parameters = null)
#pragma warning restore CA1019 // Define accessors for attribute arguments
        {
            ServiceLifetime = serviceLifetime;
            if (!string.IsNullOrEmpty(parameters))
            {
                Parameters = parameters.Split(Constants.Delimiter, StringSplitOptions.RemoveEmptyEntries)?.Select(t => Type.GetType(t));
            }
        }

        public ServiceLifetime ServiceLifetime { get; }

        public IEnumerable<Type> Parameters { get; }
    }
}

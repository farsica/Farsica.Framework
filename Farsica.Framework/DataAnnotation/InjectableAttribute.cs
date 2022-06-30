﻿namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class InjectableAttribute : Attribute
    {
    }
}

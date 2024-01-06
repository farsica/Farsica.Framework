﻿namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScaffoldColumnAttribute(bool scaffold) : System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute(scaffold)
    {
    }
}

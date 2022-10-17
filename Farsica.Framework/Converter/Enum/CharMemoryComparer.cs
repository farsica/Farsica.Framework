﻿namespace Farsica.Framework.Converter.Enum
{
    using System;
    using System.Collections.Generic;

    public class CharMemoryComparer : IEqualityComparer<ReadOnlyMemory<char>>
    {
        private readonly StringComparison comparison;

        private CharMemoryComparer(StringComparison comparison) => this.comparison = comparison;

        public static CharMemoryComparer OrdinalIgnoreCase { get; } = new CharMemoryComparer(StringComparison.OrdinalIgnoreCase);

        public static CharMemoryComparer Ordinal { get; } = new CharMemoryComparer(StringComparison.Ordinal);

        public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
        {
            return MemoryExtensions.Equals(x.Span, y.Span, comparison);
        }

        public int GetHashCode(ReadOnlyMemory<char> obj)
        {
#pragma warning disable RS1024 // Compare symbols correctly
            return string.GetHashCode(obj.Span, comparison);
#pragma warning restore RS1024 // Compare symbols correctly
        }
    }
}

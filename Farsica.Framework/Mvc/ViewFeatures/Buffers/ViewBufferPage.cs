namespace Farsica.Framework.Mvc.ViewFeatures.Buffers
{
    using System.Runtime.CompilerServices;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

    internal class ViewBufferPage(ViewBufferValue[] buffer)
    {
        public ViewBufferValue[] Buffer { get; } = buffer;

        public int Capacity => Buffer.Length;

        public int Count { get; set; }

        public bool IsFull => Count == Capacity;

        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(ViewBufferValue value) => Buffer[Count++] = value;
    }
}

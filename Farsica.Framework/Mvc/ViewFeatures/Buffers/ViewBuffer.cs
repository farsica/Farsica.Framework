namespace Farsica.Framework.Mvc.ViewFeatures.Buffers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

    [DebuggerDisplay("{DebuggerToString()}")]
    internal class ViewBuffer : IHtmlContentBuilder
    {
        public static readonly int PartialViewPageSize = 32;
        public static readonly int TagHelperPageSize = 32;
        public static readonly int ViewComponentPageSize = 32;
        public static readonly int ViewPageSize = 256;

        private readonly IViewBufferScope bufferScope;
        private readonly string? name;
        private readonly int pageSize;
        private ViewBufferPage currentPage;         // Limits allocation if the ViewBuffer has only one page (frequent case).
        private List<ViewBufferPage> multiplePages; // Allocated only if necessary

        public ViewBuffer(IViewBufferScope bufferScope, string? name, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            this.bufferScope = bufferScope ?? throw new ArgumentNullException(nameof(bufferScope));
            this.name = name;
            this.pageSize = pageSize;
        }

        public int Count
        {
            get
            {
                if (multiplePages != null)
                {
                    return multiplePages.Count;
                }

                if (currentPage != null)
                {
                    return 1;
                }

                return 0;
            }
        }

        public ViewBufferPage this[int index]
        {
            get
            {
                if (multiplePages != null)
                {
                    return multiplePages[index];
                }

                if (index == 0 && currentPage != null)
                {
                    return currentPage;
                }

                throw new IndexOutOfRangeException();
            }
        }

        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IHtmlContentBuilder Append(string unencoded)
        {
            if (unencoded != null)
            {
                // Text that needs encoding is the uncommon case in views, which is why it
                // creates a wrapper and pre-encoded text does not.
                AppendValue(new ViewBufferValue(new EncodingWrapper(unencoded)));
            }

            return this;
        }

        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IHtmlContentBuilder AppendHtml(IHtmlContent content)
        {
            if (content != null)
            {
                AppendValue(new ViewBufferValue(content));
            }

            return this;
        }

        /// <inheritdoc />
        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IHtmlContentBuilder AppendHtml(string encoded)
        {
            if (encoded != null)
            {
                AppendValue(new ViewBufferValue(encoded));
            }

            return this;
        }

        /// <inheritdoc />
        public IHtmlContentBuilder Clear()
        {
            multiplePages = null;
            currentPage = null;
            return this;
        }

        /// <inheritdoc />
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];

                    if (value.Value is string valueAsString)
                    {
                        writer.Write(valueAsString);
                        continue;
                    }

                    if (value.Value is IHtmlContent valueAsHtmlContent)
                    {
                        valueAsHtmlContent.WriteTo(writer, encoder);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Writes the buffered content to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/>.</param>
        /// <param name="encoder">The <see cref="HtmlEncoder"/>.</param>
        /// <returns>A <see cref="Task"/> which will complete once content has been written.</returns>
        public async Task WriteToAsync(TextWriter writer, HtmlEncoder encoder)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];

                    if (value.Value is string valueAsString)
                    {
                        await writer.WriteAsync(valueAsString);
                        continue;
                    }

                    if (value.Value is ViewBuffer valueAsViewBuffer)
                    {
                        await valueAsViewBuffer.WriteToAsync(writer, encoder);
                        continue;
                    }

                    if (value.Value is IHtmlContent valueAsHtmlContent)
                    {
                        valueAsHtmlContent.WriteTo(writer, encoder);
                        await writer.FlushAsync();
                        continue;
                    }
                }
            }
        }

        public void CopyTo(IHtmlContentBuilder destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];
                    if (value.Value is string valueAsString)
                    {
                        destination.AppendHtml(valueAsString);
                    }
                    else if (value.Value is IHtmlContentContainer valueAsContainer)
                    {
                        valueAsContainer.CopyTo(destination);
                    }
                    else
                    {
                        destination.AppendHtml((IHtmlContent)value.Value);
                    }
                }
            }
        }

        public void MoveTo(IHtmlContentBuilder destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            // Perf: We have an efficient implementation when the destination is another view buffer,
            // we can just insert our pages as-is.
            if (destination is ViewBuffer other)
            {
                MoveTo(other);
                return;
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];
                    if (value.Value is string valueAsString)
                    {
                        destination.AppendHtml(valueAsString);
                    }
                    else if (value.Value is IHtmlContentContainer valueAsContainer)
                    {
                        valueAsContainer.MoveTo(destination);
                    }
                    else
                    {
                        destination.AppendHtml((IHtmlContent)value.Value);
                    }
                }
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                Array.Clear(page.Buffer, 0, page.Count);
                bufferScope.ReturnSegment(page.Buffer);
            }

            Clear();
        }

        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendValue(ViewBufferValue value)
        {
            var page = GetCurrentPage();
            page.Append(value);
        }

        // Very common trivial method; nudge it to inline https://github.com/aspnet/Mvc/pull/8339
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ViewBufferPage GetCurrentPage()
        {
            var currentPage = this.currentPage;
            if (currentPage == null || currentPage.IsFull)
            {
                // Uncommon slow-path
                return AppendNewPage();
            }

            return currentPage;
        }

        // Slow path for above, don't inline
        [MethodImpl(MethodImplOptions.NoInlining)]
        private ViewBufferPage AppendNewPage()
        {
            AddPage(new ViewBufferPage(bufferScope.GetPage(pageSize)));
            return currentPage;
        }

        private void AddPage(ViewBufferPage page)
        {
            if (multiplePages != null)
            {
                multiplePages.Add(page);
            }
            else if (currentPage != null)
            {
                multiplePages = new List<ViewBufferPage>(2)
                {
                    currentPage,
                    page,
                };
            }

            currentPage = page;
        }

        private void MoveTo(ViewBuffer destination)
        {
            for (var i = 0; i < Count; i++)
            {
                var page = this[i];

                var destinationPage = destination.Count == 0 ? null : destination[destination.Count - 1];

                // If the source page is less or equal to than half full, let's copy it's content to the destination
                // page if possible.
                var isLessThanHalfFull = 2 * page.Count <= page.Capacity;
                if (isLessThanHalfFull &&
                    destinationPage != null &&
                    destinationPage.Capacity - destinationPage.Count >= page.Count)
                {
                    // We have room, let's copy the items.
                    Array.Copy(
                        sourceArray: page.Buffer,
                        sourceIndex: 0,
                        destinationArray: destinationPage.Buffer,
                        destinationIndex: destinationPage.Count,
                        length: page.Count);

                    destinationPage.Count += page.Count;

                    // Now we can return the source page, and it can be reused in the scope of this request.
                    Array.Clear(page.Buffer, 0, page.Count);
                    bufferScope.ReturnSegment(page.Buffer);
                }
                else
                {
                    // Otherwise, let's just add the source page to the other buffer.
                    destination.AddPage(page);
                }
            }

            Clear();
        }

        private class EncodingWrapper : IHtmlContent
        {
            private readonly string? unencoded;

            public EncodingWrapper(string unencoded)
            {
                this.unencoded = unencoded;
            }

            public void WriteTo(TextWriter writer, HtmlEncoder encoder)
            {
                encoder.Encode(writer, unencoded);
            }
        }
    }
}

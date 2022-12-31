namespace Farsica.Framework.Mvc.ViewFeatures.Buffers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Html;

    internal class ViewBufferTextWriter : TextWriter
    {
        private readonly TextWriter inner;
        private readonly HtmlEncoder htmlEncoder;

        /// <summary>
        /// Creates a new instance of <see cref="ViewBufferTextWriter"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="ViewBuffer"/> for buffered output.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/>.</param>
        public ViewBufferTextWriter(ViewBuffer buffer, Encoding encoding)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewBufferTextWriter"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="ViewBuffer"/> for buffered output.</param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/>.</param>
        /// <param name="htmlEncoder">The HTML encoder.</param>
        /// <param name="inner">
        /// The inner <see cref="TextWriter"/> to write output to when this instance is no longer buffering.
        /// </param>
        public ViewBufferTextWriter(ViewBuffer buffer, Encoding encoding, HtmlEncoder htmlEncoder, TextWriter inner)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            this.htmlEncoder = htmlEncoder ?? throw new ArgumentNullException(nameof(htmlEncoder));
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <inheritdoc />
        public override Encoding Encoding { get; }

        /// <summary>
        /// Gets the <see cref="ViewBuffer"/>.
        /// </summary>
        public ViewBuffer Buffer { get; }

        /// <summary>
        /// Gets a value that indiciates if <see cref="Flush"/> or <see cref="FlushAsync" /> was invoked.
        /// </summary>
        public bool Flushed { get; private set; }

        /// <inheritdoc />
        public override void Write(char value)
        {
            Buffer.AppendHtml(value.ToString());
        }

        /// <inheritdoc />
        public override void Write(char[] buffer, int index, int count)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (index < 0 || index >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || (buffer.Length - index < count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Buffer.AppendHtml(new string(buffer, index, count));
        }

        /// <inheritdoc />
        public override void Write(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            Buffer.AppendHtml(value);
        }

        /// <inheritdoc />
        public override void Write(object value)
        {
            if (value is null)
            {
                return;
            }

            if (value is IHtmlContentContainer container)
            {
                Write(container);
            }
            else if (value is IHtmlContent htmlContent)
            {
                Write(htmlContent);
            }
            else
            {
                Write(value.ToString());
            }
        }

        /// <summary>
        /// Writes an <see cref="IHtmlContent"/> value.
        /// </summary>
        /// <param name="value">The <see cref="IHtmlContent"/> value.</param>
        public void Write(IHtmlContent value)
        {
            if (value is null)
            {
                return;
            }

            Buffer.AppendHtml(value);
        }

        /// <summary>
        /// Writes an <see cref="IHtmlContentContainer"/> value.
        /// </summary>
        /// <param name="value">The <see cref="IHtmlContentContainer"/> value.</param>
        public void Write(IHtmlContentContainer value)
        {
            if (value is null)
            {
                return;
            }

            value.MoveTo(Buffer);
        }

        /// <inheritdoc />
        public override void WriteLine(object value)
        {
            if (value is null)
            {
                return;
            }

            if (value is IHtmlContentContainer container)
            {
                Write(container);
                Write(NewLine);
            }
            else if (value is IHtmlContent htmlContent)
            {
                Write(htmlContent);
                Write(NewLine);
            }
            else
            {
                Write(value.ToString());
                Write(NewLine);
            }
        }

        /// <inheritdoc />
        public override Task WriteAsync(char value)
        {
            Buffer.AppendHtml(value.ToString());
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || (buffer.Length - index < count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Buffer.AppendHtml(new string(buffer, index, count));
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteAsync(string value)
        {
            Buffer.AppendHtml(value);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override void WriteLine()
        {
            Buffer.AppendHtml(NewLine);
        }

        /// <inheritdoc />
        public override void WriteLine(string value)
        {
            Buffer.AppendHtml(value);
            Buffer.AppendHtml(NewLine);
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(char value)
        {
            Buffer.AppendHtml(value.ToString());
            Buffer.AppendHtml(NewLine);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(char[] value, int start, int offset)
        {
            Buffer.AppendHtml(new string(value, start, offset));
            Buffer.AppendHtml(NewLine);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(string value)
        {
            Buffer.AppendHtml(value);
            Buffer.AppendHtml(NewLine);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLineAsync()
        {
            Buffer.AppendHtml(NewLine);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Copies the buffered content to the unbuffered writer and invokes flush on it.
        /// </summary>
        public override void Flush()
        {
            if (inner is null or ViewBufferTextWriter)
            {
                return;
            }

            Flushed = true;

            Buffer.WriteTo(inner, htmlEncoder);
            Buffer.Clear();

            inner.Flush();
        }

        /// <summary>
        /// Copies the buffered content to the unbuffered writer and invokes flush on it.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous copy and flush operations.</returns>
        public override async Task FlushAsync()
        {
            if (inner is null or ViewBufferTextWriter)
            {
                return;
            }

            Flushed = true;

            await Buffer.WriteToAsync(inner, htmlEncoder);
            Buffer.Clear();

            await inner.FlushAsync();
        }
    }
}

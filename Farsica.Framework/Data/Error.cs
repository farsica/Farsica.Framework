namespace Farsica.Framework.Data
{
    using System;

    public struct Error(Exception exception)
    {
        public string? Message { get; set; } = exception.Message;

        public string? Code { get; set; }

        public string? Reference { get; set; }

        public string? Info { get; set; }

        public object? Value { get; set; }
    }
}

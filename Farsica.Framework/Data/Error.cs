﻿namespace Farsica.Framework.Data
{
    using System;
    using System.Text.RegularExpressions;
    using Farsica.Framework.Core;

    public partial struct Error(Exception? exception)
    {
        public static readonly Regex CodeRegex = CodeRegexPartial();

        private string? message = exception?.Message;

        public string? Message
        {
            get
            {
                return message;
            }

            set
            {
                if (value is null)
                {
                    message = value;
                    return;
                }

                var match = CodeRegex.Match(value);
                if (match.Success)
                {
                    message = value.Replace(match.Value, string.Empty);
                    Code = Constants.ErrorCodePrefix + match.Value.Replace("*", string.Empty);
                }
                else
                {
                    message = value;
                }
            }
        }

        public string? Code { get; set; }

        public string? Reference { get; set; }

        public string? Info { get; set; }

        public object? Value { get; set; }

        public static string FormatMessage(string? msg)
        {
            if (msg is null)
            {
                return string.Empty;
            }

            var match = CodeRegex.Match(msg);
            if (match.Success)
            {
                return msg.Replace(match.Value, string.Empty);
            }

            return msg;
        }

        [GeneratedRegex("\\*\\*\\d{3}\\*\\*")]
        private static partial Regex CodeRegexPartial();
    }
}

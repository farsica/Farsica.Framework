namespace Farsica.Framework.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using Farsica.Framework.Core.Extensions.Collections.Generic;

    public static class StringExtensions
    {
        public static string? EnsureEndsWith(this string? str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str?.EndsWith(c.ToString(), comparisonType) is true)
            {
                return str;
            }

            return str + c;
        }

        public static string? EnsureStartsWith(this string? str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str?.StartsWith(c.ToString(), comparisonType) is true)
            {
                return str;
            }

            return c + str;
        }

        public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);

        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string? Left(this string? str, int len)
        {
            if (str?.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str?[..len];
        }

        public static string? NormalizeLineEndings(this string? str)
        {
            return str?.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        public static int NthIndexOf(this string str, char c, int n)
        {
            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string? or the same string? if it has not any of given postfixes.</returns>
        public static string? RemovePostFix(this string? str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string? or the same string? if it has not any of given postfixes.</returns>
        public static string? RemovePostFix(this string? str, StringComparison comparisonType, params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            if (postFixes is null)
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str?.EndsWith(postFix, comparisonType) is true)
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string? or the same string? if it has not any of given prefixes.</returns>
        public static string? RemovePreFix(this string? str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string? or the same string? if it has not any of given prefixes.</returns>
        public static string? RemovePreFix(this string? str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (preFixes is null || preFixes.IsNullOrEmpty())
            {
                return str;
            }

            if (preFixes is not null)
            {
                foreach (var preFix in preFixes)
                {
                    if (str?.StartsWith(preFix, comparisonType) is true)
                    {
                        return str.Right(str.Length - preFix.Length);
                    }
                }
            }

            return str;
        }

        public static string? ReplaceFirst(this string str, string search, string? replace, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var pos = str.IndexOf(search, comparisonType);
            if (pos < 0)
            {
                return str;
            }

            return str[..pos] + replace + str[(pos + search.Length)..];
        }

        public static string? Right(this string str, int len)
        {
            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        public static string[]? Split(this string? str, string? separator)
        {
            return str?.Split(new[] { separator }, StringSplitOptions.None);
        }

        public static string[]? Split(this string? str, string? separator, StringSplitOptions options)
        {
            return str?.Split(new[] { separator }, options);
        }

        public static IEnumerable<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> chars, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            int index;
            while ((index = chars.Span.IndexOf(separator)) >= 0)
            {
                var slice = chars[..index];
                if ((options & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries)
                {
                    slice = slice.Trim();
                }

                if ((options & StringSplitOptions.RemoveEmptyEntries) == 0 || slice.Length > 0)
                {
                    yield return slice;
                }

                chars = chars[(index + 1)..];
            }

            if ((options & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries)
            {
                chars = chars.Trim();
            }

            if ((options & StringSplitOptions.RemoveEmptyEntries) == 0 || chars.Length > 0)
            {
                yield return chars;
            }
        }

        public static string[]? SplitToLines(this string? str)
        {
            return str?.Split(Environment.NewLine);
        }

        public static string[]? SplitToLines(this string? str, StringSplitOptions options)
        {
            return str?.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string? to camelCase string.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>camelCase of the string.</returns>
        public static string? ToCamelCase(this string? str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str[1..];
        }

        public static string? ToSentenceCase(this string? str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
        }

        public static T? ToEnum<T>(this string? value)
            where T : struct
        {
            return string.IsNullOrEmpty(value) ? null : (T)Enum.Parse(typeof(T), value);
        }

        public static T? ToEnum<T>(this string? value, bool ignoreCase)
            where T : struct
        {
            return string.IsNullOrEmpty(value) ? null : (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string? ToMd5(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts camelCase string? to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>PascalCase of the string.</returns>
        public static string? ToPascalCase(this string? str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str[1..];
        }

        public static string? Truncate(this string? str, int maxLength)
        {
            if (str is null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        public static string? TruncateFromBeginning(this string? str, int maxLength)
        {
            if (str is null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Right(maxLength);
        }

        public static string? TruncateWithPostfix(this string? str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        public static string? TruncateWithPostfix(this string? str, int maxLength, string postfix)
        {
            if (str is null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        public static byte[] GetBytes([NotNull] this string str, [NotNull] Encoding encoding)
        {
            return encoding.GetBytes(str);
        }
    }
}

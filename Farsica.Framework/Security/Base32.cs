﻿namespace Farsica.Framework.Security
{
    using System;
    using System.Text;

    public static class Base32
    {
        private static readonly string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string? ToBase32(byte[] input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            StringBuilder sb = new();
            for (int offset = 0; offset < input.Length;)
            {
                int numCharsToOutput = GetNextGroup(input, ref offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append((numCharsToOutput >= 1) ? Base32Chars[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? Base32Chars[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? Base32Chars[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? Base32Chars[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? Base32Chars[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? Base32Chars[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? Base32Chars[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? Base32Chars[h] : '=');
            }

            return sb.ToString();
        }

        public static byte[] FromBase32(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.TrimEnd('=').ToUpperInvariant();
            if (input.Length == 0)
            {
                return Array.Empty<byte>();
            }

            var output = new byte[input.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;
            while (outputIndex < output.Length)
            {
                var byteIndex = Base32Chars.IndexOf(input[inputIndex]);
                if (byteIndex < 0)
                {
                    throw new FormatException();
                }

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                output[outputIndex] <<= bits;
                output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }

            return output;
        }

        // returns the number of bytes that were output
        private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
        {
            uint b1, b2, b3, b4, b5;
            var retVal = (offset - input.Length) switch
            {
                1 => 2,
                2 => 4,
                3 => 5,
                4 => 7,
                _ => 8,
            };
            b1 = (offset < input.Length) ? input[offset++] : 0U;
            b2 = (offset < input.Length) ? input[offset++] : 0U;
            b3 = (offset < input.Length) ? input[offset++] : 0U;
            b4 = (offset < input.Length) ? input[offset++] : 0U;
            b5 = (offset < input.Length) ? input[offset++] : 0U;

            a = (byte)(b1 >> 3);
            b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
            c = (byte)((b2 >> 1) & 0x1f);
            d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
            e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
            f = (byte)((b4 >> 2) & 0x1f);
            g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
            h = (byte)(b5 & 0x1f);

            return retVal;
        }
    }
}

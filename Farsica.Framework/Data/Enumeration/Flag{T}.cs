namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Collections;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public class Flag<T>
    {
        private readonly BitArray bits;

        public Flag(int index, int? length = null)
        {
            index++;
            length ??= index + 1;

            // None
            if (index == 0)
            {
                bits = new BitArray(length.Value, false);
                return;
            }

            // Items
            bits = new BitArray(length.Value, false);
            bits.Set(index - 1, true);
        }

        public Flag(BitArray new_value)
        {
            bits = new_value;
        }

        public Flag(byte[] new_value)
        {
            bits = new BitArray(new_value);
        }

        public Flag()
        {
            bits = new BitArray(1, false);
        }

        public int Length => bits.Length;

        public static explicit operator Flag(Flag<T> item)
        {
            return new Flag(item.bits);
        }

        public static explicit operator Flag<T>(Flag item)
        {
            return new Flag<T>(item.bits);
        }

        public static bool operator ==(Flag<T> item, Flag<T> item2)
        {
            return item.Equals(item2);
        }

        public static bool operator !=(Flag<T> item, Flag<T> item2)
        {
            return !(item == item2);
        }

        public static Flag<T> operator |(Flag<T> left, Flag<T> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new Flag<T>(nLeft.Or(nRight));
        }

        public static Flag<T> operator &(Flag<T> left, Flag<T> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new Flag<T>(nLeft.And(nRight));
        }

        public static Flag<T> operator ^(Flag<T> left, Flag<T> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new Flag<T>(nLeft.Xor(nRight));
        }

        public static Flag<T> operator ~(Flag<T> item)
        {
            var x = (BitArray)item.bits.Clone();
            return new Flag<T>(x.Not());
        }

        public override string ToString() => ToUniqueId();

        public string ToUniqueId()
        {
            var data = ToBytes();
            using var compressedStream = new MemoryStream();
            using var zipStream = new DeflateStream(compressedStream, CompressionLevel.Fastest);
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return Convert.ToBase64String(compressedStream.ToArray());
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Flag<T> item)
            {
                return false;
            }

            if (item.bits.Length > bits.Length)
            {
                return SequenceEqual(item, this);
            }

            if (item.bits.Length < bits.Length)
            {
                return SequenceEqual(this, item);
            }

            return item.bits.Length == bits.Length && ((BitArray)item.bits.Clone()).Xor(bits).OfType<bool>().All(e => !e);

            static bool SequenceEqual(Flag<T> bigger, Flag<T> smaller)
            {
                var bytes = new byte[((bigger.bits.Length - 1) / 8) + 1];
                smaller.bits.CopyTo(bytes, 0);
                return bigger.ToBytes().SequenceEqual(bytes);
            }
        }

        public override int GetHashCode()
        {
            return bits.GetHashCode() * 31;
        }

        public byte[] ToBytes()
        {
            var ret = new byte[((bits.Length - 1) / 8) + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        private static (BitArray Left, BitArray Right) FixLength(Flag<T> left, Flag<T> right)
        {
            var length = Math.Max(left.bits.Length, right.bits.Length);
            var nLeft = new BitArray(length, false);

            for (var i = 0; i < left.bits.Length; i++)
            {
                if (left.bits[i])
                {
                    nLeft.Set(i, true);
                }
            }

            var nRight = new BitArray(length, false);
            for (var i = 0; i < right.bits.Length; i++)
            {
                if (right.bits[i])
                {
                    nRight.Set(i, true);
                }
            }

            return (nLeft, nRight);
        }
    }
}

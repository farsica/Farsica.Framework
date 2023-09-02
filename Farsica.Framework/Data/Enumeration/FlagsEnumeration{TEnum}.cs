namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Collections;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using Farsica.Framework.Core;

    public abstract class FlagsEnumeration<TEnum>
        where TEnum : FlagsEnumeration<TEnum>, new()
    {
        protected FlagsEnumeration()
        {
            Bits = new BitArray(1, false);
        }

        protected FlagsEnumeration(int index)
        {
            index++;
            var length = index + 1;

            // None
            if (index == 0)
            {
                Bits = new BitArray(length, false);
                return;
            }

            // Items
            Bits = new BitArray(length, false);
            Bits.Set(index - 1, true);
        }

        protected FlagsEnumeration(BitArray value)
        {
            Bits = new BitArray(value);
        }

        protected FlagsEnumeration(byte[] value)
        {
            Bits = new BitArray(value);
        }

        public int Length => Bits.Length;

        public BitArray Bits { get; init; }

        public string? LocalizedDisplayName
        {
            get
            {
                var names = this.GetNames();
                return string.Join(", ", GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(t => names!.Contains(t.Name)).Select(Globals.GetLocalizedDisplayName));
            }
        }

        public string? LocalizedShortName
        {
            get
            {
                var names = this.GetNames();
                return string.Join(", ", GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(t => names!.Contains(t.Name)).Select(Globals.GetLocalizedShortName));
            }
        }

        public string? LocalizedDescription
        {
            get
            {
                var names = this.GetNames();
                return string.Join(", ", GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(t => names!.Contains(t.Name)).Select(Globals.GetLocalizedDescription));
            }
        }

        public string? LocalizedPromt
        {
            get
            {
                var names = this.GetNames();
                return string.Join(", ", GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(t => names!.Contains(t.Name)).Select(Globals.GetLocalizedPromt));
            }
        }

        public string? LocalizedGroupName
        {
            get
            {
                var names = this.GetNames();
                return string.Join(", ", GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(t => names!.Contains(t.Name)).Select(Globals.GetLocalizedGroupName));
            }
        }

        public static bool operator ==(FlagsEnumeration<TEnum> item, FlagsEnumeration<TEnum> item2)
        {
            return item.Equals(item2);
        }

        public static bool operator !=(FlagsEnumeration<TEnum> item, FlagsEnumeration<TEnum> item2)
        {
            return !(item == item2);
        }

        public static TEnum operator |(FlagsEnumeration<TEnum> left, FlagsEnumeration<TEnum> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new TEnum { Bits = nLeft.Or(nRight) };
        }

        public static TEnum operator &(FlagsEnumeration<TEnum> left, FlagsEnumeration<TEnum> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new TEnum { Bits = nLeft.And(nRight) };
        }

        public static TEnum operator ^(FlagsEnumeration<TEnum> left, FlagsEnumeration<TEnum> right)
        {
            var (nLeft, nRight) = FixLength(left, right);
            return new TEnum { Bits = nLeft.Xor(nRight) };
        }

        public static TEnum operator ~(FlagsEnumeration<TEnum> item)
        {
            var x = (BitArray)item.Bits.Clone();
            return new TEnum { Bits = x.Not() };
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
            if (obj is not TEnum item)
            {
                return false;
            }

            if (item.Bits.Length > Bits.Length)
            {
                return SequenceEqual(item, this);
            }

            if (item.Bits.Length < Bits.Length)
            {
                return SequenceEqual(this, item);
            }

            return item.Bits.Length == Bits.Length && ((BitArray)item.Bits.Clone()).Xor(Bits).OfType<bool>().All(e => !e);

            static bool SequenceEqual(FlagsEnumeration<TEnum> bigger, FlagsEnumeration<TEnum> smaller)
            {
                var bytes = new byte[((bigger.Bits.Length - 1) / 8) + 1];
                smaller.Bits.CopyTo(bytes, 0);
                return bigger.ToBytes().SequenceEqual(bytes);
            }
        }

        public override int GetHashCode()
        {
            return Bits.GetHashCode() * 31;
        }

        public byte[] ToBytes()
        {
            var ret = new byte[((Bits.Length - 1) / 8) + 1];
            Bits.CopyTo(ret, 0);
            return ret;
        }

        private static (BitArray Left, BitArray Right) FixLength(FlagsEnumeration<TEnum> left, FlagsEnumeration<TEnum> right)
        {
            var length = Math.Max(left.Bits.Length, right.Bits.Length);
            var nLeft = new BitArray(length, false);

            for (var i = 0; i < left.Bits.Length; i++)
            {
                if (left.Bits[i])
                {
                    nLeft.Set(i, true);
                }
            }

            var nRight = new BitArray(length, false);
            for (var i = 0; i < right.Bits.Length; i++)
            {
                if (right.Bits[i])
                {
                    nRight.Set(i, true);
                }
            }

            return (nLeft, nRight);
        }
    }
}

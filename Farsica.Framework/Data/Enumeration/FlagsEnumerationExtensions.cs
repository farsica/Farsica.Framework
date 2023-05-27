namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class FlagsEnumerationExtensions
    {
        public static IEnumerable<string> GetNames<TEnum>()
            where TEnum : FlagsEnumeration<TEnum>
        {
            return GetNames<TEnum>(BindingFlags.Public | BindingFlags.Static);
        }

        public static IEnumerable<string> GetNames<TEnum>(BindingFlags bindingFlags)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return typeof(TEnum)
                .GetFields(bindingFlags)
                .Where(f => f.FieldType == typeof(Flag<TEnum>))
                .Select(f => f.Name);
        }

        public static IEnumerable<string> GetNames<TEnum>(Flag<TEnum> enumFlag, BindingFlags bindingFlag)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return GetKeyValues<TEnum>(bindingFlag).Where(x => enumFlag.HasFlag(x.Value)).Select(x => x.Key);
        }

        public static IEnumerable<string> GetNames<TEnum>(Flag<TEnum> enumFlag)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return GetNames<TEnum>(enumFlag, BindingFlags.Public | BindingFlags.Static);
        }

        public static Dictionary<string, Flag<TEnum>> GetKeyValues<TEnum>()
            where TEnum : FlagsEnumeration<TEnum>
        {
            return GetKeyValues<TEnum>(BindingFlags.Public | BindingFlags.Static);
        }

        public static Dictionary<string, Flag<TEnum>> GetKeyValues<TEnum>(BindingFlags bindingFlags)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return typeof(TEnum)
                .GetFields(bindingFlags)
                .Where(f => f.FieldType == typeof(Flag<TEnum>))
                .ToDictionary(f => f.Name, f => (Flag<TEnum>)f.GetValue(null)!);
        }

        public static Flag<TEnum>? FromName<TEnum>(string name)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return FromName<TEnum>(name, BindingFlags.Public | BindingFlags.Static);
        }

        public static Flag<TEnum>? FromName<TEnum>(string name, BindingFlags bindingFlags)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return typeof(TEnum).GetField(name, bindingFlags)?.GetValue(null) as Flag<TEnum>;
        }

        public static Flag<TEnum> GetAll<TEnum>()
            where TEnum : FlagsEnumeration<TEnum>
        {
            var count = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Count(f => f.FieldType == typeof(Flag<TEnum>) || f.FieldType.BaseType == typeof(Flag<TEnum>));
            return new Flag<TEnum>(new BitArray(count - 1, true));
        }

        public static Flag<TEnum> FromUniqueId<TEnum>(this string id)
            where TEnum : FlagsEnumeration<TEnum>
        {
            var data = Convert.FromBase64String(id);
            using var compressedStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();
            using var zipStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            zipStream.CopyTo(outputStream);
            zipStream.Close();
            var bytes = outputStream.ToArray();

            return new Flag<TEnum>(bytes);
        }

        public static bool HasFlag<TEnum>(this Flag<TEnum> left, Flag<TEnum> right)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return (left & right) != new Flag<TEnum>(-1);
        }

        public static Flag<TEnum> SetFlag<TEnum>(this Flag<TEnum> left, params Flag<TEnum>[] right)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return right.Aggregate(left, (current, item) => current | item);
        }

        public static Flag<TEnum> UnsetFlag<TEnum>(this Flag<TEnum> left, params Flag<TEnum>[] right)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return right.Aggregate(left, (current, item) => current & ~item);
        }

        public static Flag<TEnum> ToggleFlag<TEnum>(this Flag<TEnum> left, params Flag<TEnum>[] right)
            where TEnum : FlagsEnumeration<TEnum>
        {
            return right.Aggregate(left, (current, item) => current ^ item);
        }

        public static PropertyBuilder<Flag<TEnum>?> OwnFlagsEnumeration<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, Flag<TEnum>?>> property)
            where TEntity : class
            where TEnum : FlagsEnumeration<TEnum>
        {
            return builder.Property(property).HasConversion(t => t.ToUniqueId(), t => t.FromUniqueId<TEnum>());
        }
    }
}

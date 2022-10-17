namespace Farsica.Framework.Converter.Enum
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.Core.Extensions.Collections;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public readonly record struct EnumData<TEnum>(ReadOnlyMemory<char> Name, TEnum Value, ulong UInt64Value)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        where TEnum : struct, Enum;

    public delegate bool TryOverrideName(Type enumType, string name, out ReadOnlyMemory<char> overrideName);

    public class GeneralJsonStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonNamingPolicy? namingPolicy;
        private readonly bool allowIntegerValues;

        public GeneralJsonStringEnumConverter()
            : this(null, true)
        {
        }

        public GeneralJsonStringEnumConverter(JsonNamingPolicy? namingPolicy = default, bool allowIntegerValues = true) => (this.namingPolicy, this.allowIntegerValues) = (namingPolicy, allowIntegerValues);

        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum || Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;

        public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var flagged = enumType.IsDefined(typeof(FlagsAttribute), true);
            JsonConverter enumConverter;
            TryOverrideName tryOverrideName = (Type t, string n, out ReadOnlyMemory<char> o) => TryOverrideName(t, n, out o);
            var converterType = (flagged ? typeof(FlaggedJsonEnumConverter<>) : typeof(UnflaggedJsonEnumConverter<>)).MakeGenericType(new[] { enumType });
            enumConverter = (JsonConverter)Activator.CreateInstance(
                converterType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: new object[] { namingPolicy!, allowIntegerValues, tryOverrideName },
                culture: null)!;
            if (enumType == typeToConvert)
            {
                return enumConverter;
            }
            else
            {
                var nullableConverter = (JsonConverter)Activator.CreateInstance(
                    typeof(NullableConverterDecorator<>).MakeGenericType(new[] { enumType }),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    binder: null,
                    args: new object[] { enumConverter },
                    culture: null)!;
                return nullableConverter;
            }
        }

        protected virtual bool TryOverrideName(Type enumType, string name, out ReadOnlyMemory<char> overrideName)
        {
            overrideName = default;
            return false;
        }

        public class FlaggedJsonEnumConverter<TEnum> : JsonEnumConverterBase<TEnum>
            where TEnum : struct, Enum
        {
            private const char FlagSeparatorChar = ',';
            private const string FlagSeparatorString = ", ";

            public FlaggedJsonEnumConverter(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName)
                : base(namingPolicy, allowNumbers, tryOverrideName)
            {
            }

            protected override bool TryFormatAsString(EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name)
            {
                ulong uInt64Value = JsonEnumExtensions.ToUInt64(value, EnumTypeCode);
                var index = enumData.BinarySearchFirst(uInt64Value, EntryComparer);
                if (index >= 0)
                {
                    // A single flag
                    name = enumData[index].Name;
                    return true;
                }

                if (uInt64Value != 0)
                {
                    StringBuilder? sb = null;
                    for (int i = (~index) - 1; i >= 0; i--)
                    {
                        if ((uInt64Value & enumData[i].UInt64Value) == enumData[i].UInt64Value && enumData[i].UInt64Value != 0)
                        {
                            if (sb == null)
                            {
                                sb = new StringBuilder();
                                sb.Append(enumData[i].Name.Span);
                            }
                            else
                            {
                                sb.Insert(0, FlagSeparatorString);
                                sb.Insert(0, enumData[i].Name.Span);
                            }

                            uInt64Value -= enumData[i].UInt64Value;
                        }
                    }

                    if (uInt64Value == 0 && sb != null)
                    {
                        name = sb.ToString().AsMemory();
                        return true;
                    }
                }

                name = default;
                return false;
            }

            protected override bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value)
            {
                ulong uInt64Value = 0;
                foreach (var slice in name.Split(FlagSeparatorChar, StringSplitOptions.TrimEntries))
                {
                    if (JsonEnumExtensions.TryLookupBest(enumData, nameLookup, slice, out TEnum thisValue))
                    {
                        uInt64Value |= thisValue.ToUInt64(EnumTypeCode);
                    }
                    else
                    {
                        value = default;
                        return false;
                    }
                }

                value = JsonEnumExtensions.FromUInt64<TEnum>(uInt64Value);
                return true;
            }
        }

        public class UnflaggedJsonEnumConverter<TEnum> : JsonEnumConverterBase<TEnum>
            where TEnum : struct, Enum
        {
            public UnflaggedJsonEnumConverter(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName)
                : base(namingPolicy, allowNumbers, tryOverrideName)
            {
            }

            protected override bool TryFormatAsString(EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name)
            {
                var index = enumData.BinarySearchFirst(JsonEnumExtensions.ToUInt64(value, EnumTypeCode), EntryComparer);
                if (index >= 0)
                {
                    name = enumData[index].Name;
                    return true;
                }

                name = default;
                return false;
            }

            protected override bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value) =>
                JsonEnumExtensions.TryLookupBest(enumData, nameLookup, name, out value);
        }

        public abstract class JsonEnumConverterBase<TEnum> : JsonConverter<TEnum>
            where TEnum : struct, Enum
        {
            protected JsonEnumConverterBase(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName)
            {
                AllowNumbers = allowNumbers;
                EnumData = JsonEnumExtensions.GetData<TEnum>(namingPolicy, tryOverrideName).ToArray();
                NameLookup = JsonEnumExtensions.GetLookupTable(EnumData);
            }

            protected static TypeCode EnumTypeCode { get; } = Type.GetTypeCode(typeof(TEnum));

            protected static Func<EnumData<TEnum>, ulong, int> EntryComparer { get; } = (item, key) => item.UInt64Value.CompareTo(key);

            private bool AllowNumbers { get; }

            private EnumData<TEnum>[] EnumData { get; }

            private ILookup<ReadOnlyMemory<char>, int> NameLookup { get; }

            public sealed override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                // Todo: consider caching a small number of JsonEncodedText values for the first N enums encountered, as is done in
                // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/EnumConverter.cs
                if (TryFormatAsString(EnumData, value, out var name))
                {
                    writer.WriteStringValue(name.Span);
                }
                else
                {
                    if (!AllowNumbers)
                    {
                        throw new JsonException();
                    }

                    WriteEnumAsNumber(writer, value);
                }
            }

            public sealed override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                reader.TokenType switch
                {
                    JsonTokenType.String => TryReadAsString(EnumData, NameLookup, reader.GetString().AsMemory(), out var value) ? value : throw new JsonException(),
                    JsonTokenType.Number => AllowNumbers ? ReadNumberAsEnum(ref reader) : throw new JsonException(),
                    _ => throw new JsonException(),
                };

            protected abstract bool TryFormatAsString(EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name);

            protected abstract bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value);

            private static void WriteEnumAsNumber(Utf8JsonWriter writer, TEnum value)
            {
                switch (EnumTypeCode)
                {
                    case TypeCode.SByte:
                        writer.WriteNumberValue(Unsafe.As<TEnum, sbyte>(ref value));
                        break;
                    case TypeCode.Int16:
                        writer.WriteNumberValue(Unsafe.As<TEnum, short>(ref value));
                        break;
                    case TypeCode.Int32:
                        writer.WriteNumberValue(Unsafe.As<TEnum, int>(ref value));
                        break;
                    case TypeCode.Int64:
                        writer.WriteNumberValue(Unsafe.As<TEnum, long>(ref value));
                        break;
                    case TypeCode.Byte:
                        writer.WriteNumberValue(Unsafe.As<TEnum, byte>(ref value));
                        break;
                    case TypeCode.UInt16:
                        writer.WriteNumberValue(Unsafe.As<TEnum, ushort>(ref value));
                        break;
                    case TypeCode.UInt32:
                        writer.WriteNumberValue(Unsafe.As<TEnum, uint>(ref value));
                        break;
                    case TypeCode.UInt64:
                        writer.WriteNumberValue(Unsafe.As<TEnum, ulong>(ref value));
                        break;
                    default:
                        throw new JsonException();
                }
            }

            private static TEnum ReadNumberAsEnum(ref Utf8JsonReader reader)
            {
                switch (EnumTypeCode)
                {
                    case TypeCode.SByte:
                        {
                            var i = reader.GetSByte();
                            return Unsafe.As<sbyte, TEnum>(ref i);
                        }

                    case TypeCode.Int16:
                        {
                            var i = reader.GetInt16();
                            return Unsafe.As<short, TEnum>(ref i);
                        }

                    case TypeCode.Int32:
                        {
                            var i = reader.GetInt32();
                            return Unsafe.As<int, TEnum>(ref i);
                        }

                    case TypeCode.Int64:
                        {
                            var i = reader.GetInt64();
                            return Unsafe.As<long, TEnum>(ref i);
                        }

                    case TypeCode.Byte:
                        {
                            var i = reader.GetByte();
                            return Unsafe.As<byte, TEnum>(ref i);
                        }

                    case TypeCode.UInt16:
                        {
                            var i = reader.GetUInt16();
                            return Unsafe.As<ushort, TEnum>(ref i);
                        }

                    case TypeCode.UInt32:
                        {
                            var i = reader.GetUInt32();
                            return Unsafe.As<uint, TEnum>(ref i);
                        }

                    case TypeCode.UInt64:
                        {
                            var i = reader.GetUInt64();
                            return Unsafe.As<ulong, TEnum>(ref i);
                        }

                    default:
                        throw new JsonException();
                }
            }
        }
    }
}

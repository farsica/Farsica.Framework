namespace Farsica.Framework.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Numerics;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Farsica.Framework.Core.Extensions;
    using Farsica.Framework.Core.Extensions.Collections;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using NUlid;

    public static partial class Globals
    {
        private static readonly char[] Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private static readonly Regex MatchIranianPostalCode = new(@"\b(?!(\d)\1{3})[13-9]{4}[1346-9][013-9]{5}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMinutes(1));

        public static CultureInfo CurrentCulture => Thread.CurrentThread.CurrentUICulture;

        public static bool IsRtl => CurrentCulture.TextInfo.IsRightToLeft;

        public static DbProviderType ProviderType { get; set; }

        public static string? NormalizePersian(this string? str)
        {
            return str?.Trim()
                .Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace("ھ", "ه")
                .Replace("ؤ", "و")
                .Replace("ة", "ه")
                .Replace("أ", "ا")
                .Replace("إ", "ا")
                .Replace("ٸ", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9");
        }

        public static string? GetClientIpAddress(this HttpContext? httpContext)
        {
            var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();
            var xForwardedFor = httpContext?.Request.Headers["X-Forwarded-For"];
            if (xForwardedFor.HasValue)
            {
                ip = xForwardedFor.Value.ToString().Split(',').Last();
            }

            return ip?.AsSpan().Contains('.') == true && ip.AsSpan().Contains(':') ? ip.AsSpan(0, ip.AsSpan().IndexOf(':')).ToString() : ip;
        }

        public static string? GetHeaderParameter(this HttpContext? httpContext, string key)
        {
            return httpContext?.Request.Headers.TryGetValue(key, out StringValues stringValues) == true ? stringValues.FirstOrDefault() : null;
        }

        public static bool ValidateNationalCode(string? nationalCode)
        {
            try
            {
                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };

                if (string.IsNullOrEmpty(nationalCode) || allDigitEqual.Contains(nationalCode) || nationalCode.Length != 10)
                {
                    return false;
                }

                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());

                var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
                var c = b % 11;

                return (c < 2 && a == c) || (c >= 2 && 11 - c == a);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePostalCode(string? postalCode)
        {
            return !string.IsNullOrEmpty(postalCode) && MatchIranianPostalCode.IsMatch(postalCode);
        }

        public static bool ValidateNationalId(string? nationalId)
        {
            try
            {
                if (string.IsNullOrEmpty(nationalId))
                {
                    return false;
                }

                nationalId = nationalId.NormalizePersian()!;
                const int initialZeros = 3;
                const int nationalIdLength = 11;

                if (nationalId.Length is < nationalIdLength - initialZeros or > nationalIdLength)
                {
                    return false;
                }

                nationalId = nationalId.PadLeft(11, '0');

                if (!nationalId.All(char.IsDigit))
                {
                    return false;
                }

                var beforeControlNumber = (int)char.GetNumericValue(nationalId[9]) + 2;
                int[] coefficientStatic = [29, 27, 23, 19, 17, 29, 27, 23, 19, 17];

                var sum = 0;
                for (var i = 0; i < nationalId.Length - 1; i++)
                {
                    sum += ((int)char.GetNumericValue(nationalId[i]) + beforeControlNumber) * coefficientStatic[i];
                }

                var remainder = sum % 11;
                var controlNumber = (int)char.GetNumericValue(nationalId[10]);
                remainder = remainder == 10 ? 0 : remainder;

                return controlNumber == remainder;
            }
            catch
            {
                return false;
            }
        }

        public static string? GetLocalizedDisplayName(MemberInfo? member)
        {
            if (member is null)
            {
                return null;
            }

            string? name;
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>(false);
            if (displayAttribute is not null)
            {
                name = GetLocalizedValueInternal(displayAttribute, member.Name, Constants.ResourceKey.Name, member: member);
                return name.IsNullOrEmpty() ? member.Name : name;
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            name = customAttribute?.GetName();
            return string.IsNullOrEmpty(name) ? member.Name : name;
        }

        public static string? DisplayNameFor<T>(this Expression<Func<T, object>> expression)
            where T : class
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression?.Member.MemberType == MemberTypes.Property)
            {
                return GetLocalizedDisplayName(memberExpression.Member);
            }

            if (expression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression?.Member.MemberType == MemberTypes.Property)
                {
                    return GetLocalizedDisplayName(memberExpression.Member);
                }
            }

            return string.Empty;
        }

        public static string? GetLocalizedShortName(MemberInfo? member)
        {
            if (member is null)
            {
                return null;
            }

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay is not null)
            {
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.ShortName, member: member);
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetShortName();
        }

        public static string? GetLocalizedDescription(MemberInfo? member)
        {
            if (member is null)
            {
                return null;
            }

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay is not null)
            {
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.Description, member: member);
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetDescription();
        }

        public static string? GetLocalizedPromt(MemberInfo? member)
        {
            if (member is null)
            {
                return null;
            }

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay is not null)
            {
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.Prompt, member: member);
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetPrompt();
        }

        public static string? GetLocalizedGroupName(MemberInfo? member)
        {
            if (member is null)
            {
                return null;
            }

            var customDisplay = member.GetCustomAttribute<DisplayAttribute>(false);
            if (customDisplay is not null)
            {
                return GetLocalizedValueInternal(customDisplay, member.Name, Constants.ResourceKey.GroupName, member: member);
            }

            var customAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(false);
            return customAttribute?.GetGroupName();
        }

        public static T? ValueOf<T>(this Dictionary<string, string> dictionary, string key, T? defaultValue = default)
        {
            dictionary.TryGetValue(key, out string? tmp);
            return tmp.ValueOf(defaultValue);
        }

        public static T? ValueOf<T>(this string? value, T? defaultValue = default)
        {
            return value.ValueOf(typeof(T), defaultValue);
        }

        public static dynamic? ValueOf(this string? value, Type type, dynamic? defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type) ?? type;
                }

                if (type.IsEnum)
                {
                    try
                    {
                        return Enum.Parse(type, value);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }

                var typeCode = Type.GetTypeCode(type);

                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        return bool.TryParse(value, out bool boolTmp) ? boolTmp : defaultValue;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                        return byte.TryParse(value, out byte byteTmp) ? byteTmp : defaultValue;

                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        return short.TryParse(value, out short shortTmp) ? shortTmp : defaultValue;

                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        return int.TryParse(value, out int intTmp) ? intTmp : defaultValue;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return long.TryParse(value, out long longTmp) ? longTmp : defaultValue;

                    case TypeCode.Single:
                        return float.TryParse(value, out float floatTmp) ? floatTmp : defaultValue;

                    case TypeCode.Double:
                        return double.TryParse(value, out double doubleTmp) ? doubleTmp : defaultValue;

                    case TypeCode.Decimal:
                        return decimal.TryParse(value, out decimal decimalTmp) ? decimalTmp : defaultValue;

                    case TypeCode.DateTime:
                        {
                            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeTmp) ? dateTimeTmp : defaultValue;
                        }

                    case TypeCode.String:
                    case TypeCode.Char:
                        return value;

                    case TypeCode.Object:
                        if (type.Name == nameof(Ulid))
                        {
                            return Ulid.TryParse(value, out Ulid ulid) ? ulid : defaultValue;
                        }

                        if (type.Name == nameof(DateTimeOffset))
                        {
                            return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffsetTmp) ? dateTimeOffsetTmp : defaultValue;
                        }

                        if (type.Name == nameof(TimeSpan))
                        {
                            return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan timeSpanTmp) ? timeSpanTmp : defaultValue;
                        }

                        if (type.Name == nameof(Guid))
                        {
                            return Guid.TryParse(value, out Guid guidTmp) ? guidTmp : defaultValue;
                        }

                        if (type.Name == nameof(TimeOnly))
                        {
                            if (TimeOnly.TryParse(value, CultureInfo.InvariantCulture, out TimeOnly timeOnlyTmp))
                            {
                                return timeOnlyTmp;
                            }

                            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeTmp) ? TimeOnly.FromDateTime(dateTimeTmp) : defaultValue;
                        }

                        if (type.Name == nameof(DateOnly))
                        {
                            if (DateOnly.TryParse(value, CultureInfo.InvariantCulture, out DateOnly dateOnlyTmp))
                            {
                                return dateOnlyTmp;
                            }

                            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeTmp) ? DateOnly.FromDateTime(dateTimeTmp) : defaultValue;
                        }

                        throw new ArgumentException(typeCode.ToString());

                    default:
                        throw new ArgumentException(typeCode.ToString());
                }
            }
            catch
            {
                throw new ArgumentException(value);
            }
        }

        public static string? UserAgent(this HttpContext? httpContext)
        {
            return httpContext?.Request.Headers.UserAgent.ToString();
        }

        public static long UserId(this HttpContext? httpContext)
        {
            return httpContext.UserId<long>();
        }

        public static T? UserId<T>(this HttpContext? httpContext)
            where T : IEquatable<T>
        {
            return (httpContext?.User).UserId<T>();
        }

        public static long UserId(this ClaimsPrincipal? claimsPrincipal)
        {
            return claimsPrincipal.UserId<long>();
        }

        public static T? UserId<T>(this ClaimsPrincipal? claimsPrincipal)
            where T : IEquatable<T>
        {
            if (claimsPrincipal is null)
            {
                return default;
            }

            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier).ValueOf<T>();
        }

        public static IDictionary<string, object?>? ObjectToDictionary(object value)
        {
            if (value is IDictionary<string, object?> dictionary)
            {
                return new Dictionary<string, object?>(dictionary, StringComparer.OrdinalIgnoreCase);
            }

            dictionary = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            if (value is not null)
            {
                dictionary = value.GetType().GetProperties().ToDictionary(t => t.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? t.Name, t => t.GetValue(value, null));
            }

            return dictionary;
        }

        public static string? GenerateRandomCode(int size = 32)
        {
            using var cryptoProvider = RandomNumberGenerator.Create();
            var secretKeyByteArray = new byte[4 * size];
            cryptoProvider.GetBytes(secretKeyByteArray);
            var result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(secretKeyByteArray, i * 4);
                var idx = rnd % Chars.Length;

                result.Append(Chars[idx]);
            }

            return result.ToString();
        }

        public static int WeekOfYear(this DateTime date, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                return new PersianCalendar().GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Saturday);
            }

            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day is >= System.DayOfWeek.Monday and <= System.DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfMonth(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                return new MyPersianCalendar().ToDateTime(int.Parse(dt.ToString("yyyy")), int.Parse(dt.ToString("MM")), 1, 0, 0, 0, 0);
            }

            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime[] GetPriodOfYear(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                var cal = new MyPersianCalendar();
                return
                [
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")), 1, 1, 0, 0, 0, 0),
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")) + 1, 1, 1, 0, 0, 0, 0).AddMilliseconds(-1),
                ];
            }

            return
                [
                    new DateTime(dt.Year, 1, 1),
                    new DateTime(dt.Year + 1, 1, 1).AddMilliseconds(-1),
                ];
        }

        public static DateTime[] GetPriodOfMonth(this DateTime dt, string cultureCode)
        {
            if (cultureCode.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                var cal = new MyPersianCalendar();
                return
                [
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")), int.Parse(dt.ToString("MM")), 1, 0, 0, 0, 0),
                    cal.ToDateTime(int.Parse(dt.ToString("yyyy")), int.Parse(dt.ToString("MM")), 1, 0, 0, 0, 0).AddMonths(1).AddDays(-1),
                ];
            }

            return
                [
                    new DateTime(dt.Year, dt.Month, 1),
                    new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1),
                ];
        }

        public static CultureInfo GetCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new CultureInfo(Constants.DefaultLanguageCode);
            }

            if (!name.StartsWith("fa", StringComparison.InvariantCultureIgnoreCase))
            {
                return new CultureInfo(name, false);
            }

            var persianCalture = new CultureInfo(name, false);
            var info = persianCalture.DateTimeFormat;
            var monthNames = new[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند", string.Empty };
            var shortestDayNames = new[] { "ى", "د", "س", "چ", "پ", "ج", "ش" };
            var dayNames = new[] { "يکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };

            info.MonthGenitiveNames = monthNames;
            info.AbbreviatedMonthGenitiveNames = monthNames;
            info.AbbreviatedMonthNames = monthNames;
            info.MonthNames = monthNames;

            info.ShortestDayNames = shortestDayNames;
            info.AbbreviatedDayNames = shortestDayNames;

            info.DayNames = dayNames;

            info.DateSeparator = "/";
            info.FullDateTimePattern = "dddd dd MMM yyyy HH:mm:ss";
            info.LongDatePattern = "dddd dd MMM yyyy";
            info.LongTimePattern = "HH:mm:ss";
            info.MonthDayPattern = "dd MMM";
            info.ShortTimePattern = "HH:mm";
            info.TimeSeparator = ":";
            info.YearMonthPattern = "MMM yyyy";
            info.AMDesignator = "ق.ظ";
            info.PMDesignator = "ب.ظ";
            info.ShortDatePattern = "yyyy/MM/dd";
            info.FirstDayOfWeek = System.DayOfWeek.Saturday;
            persianCalture.DateTimeFormat = info;

            persianCalture.NumberFormat.NumberDecimalDigits = 0;
            persianCalture.NumberFormat.CurrencyDecimalDigits = 0;
            persianCalture.NumberFormat.PercentDecimalDigits = 0;
            persianCalture.NumberFormat.CurrencyPositivePattern = 1;
            persianCalture.NumberFormat.NumberDecimalSeparator = ".";

            var persianCal = new MyPersianCalendar();

            var fieldInfo = typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            fieldInfo?.SetValue(info, persianCal);

            var field = typeof(CultureInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            field?.SetValue(persianCalture, persianCal);

            return persianCalture;
        }

        public static string? Sentencise(this string? str, bool titlecase = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            var retVal = new StringBuilder(32);

            retVal.Append(char.ToUpper(str[0]));
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsLower(str[i]))
                {
                    retVal.Append(str[i]);
                }
                else
                {
                    retVal.Append(' ');
                    if (titlecase)
                    {
                        retVal.Append(str[i]);
                    }
                    else
                    {
                        retVal.Append(char.ToLower(str[i]));
                    }
                }
            }

            return retVal.ToString();
        }

        public static IEnumerable<Type>? GetAllTypesImplementingType(this Type mainType, IEnumerable<Type> scanTypes)
        {
            IEnumerable<Type> types;

            if (mainType.IsGenericType)
            {
                types = from t1 in scanTypes
                        from t2 in t1.GetInterfaces()
                        let baseType = t1.BaseType
                        where !t1.IsAbstract &&
                        ((baseType is not null && baseType.IsGenericType && mainType.IsAssignableFrom(baseType.GetGenericTypeDefinition())) ||
                        (t2.IsGenericType && mainType.IsAssignableFrom(t2.GetGenericTypeDefinition())))
                        select t1;
            }
            else if (mainType.IsInterface)
            {
                types = scanTypes.Where(t => !t.IsAbstract && t.GetInterfaces().Contains(mainType));
            }
            else
            {
                types = scanTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(mainType));
            }

            if (!types?.Any() == true && mainType.IsClass && !mainType.IsAbstract)
            {
                types = new[] { mainType };
            }

            return types;
        }

        public static string TrimEnd(this string input, string? suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove is not null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input[..^suffixToRemove.Length];
            }

            return input;
        }

        public static string? Slugify(this string? value)
        {
            return value is null ? null : Regex.Replace(
                value,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }

        public static async Task<string?> ConvertImageToBase64Async(IFormFile file)
        {
            if (file is null)
            {
                return null;
            }

            using var target = new MemoryStream();
            await file.CopyToAsync(target);
            return $"data:{file.ContentType};base64, {Convert.ToBase64String(target.ToArray())}";
        }

        public static bool IsImage(string fileName)
        {
            return fileName.IsNullOrEmpty() is false && fileName.StartsWith("<svg", StringComparison.InvariantCultureIgnoreCase) is false && (fileName.StartsWith("data:image") || Constants.ValidImageExtensions.Contains(Path.GetExtension(fileName)?.TrimStart('.') ?? string.Empty, StringComparison.OrdinalIgnoreCase));
        }

        public static bool ValidateIban(this string? iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                return true;
            }

            if (iban.Length != 26 || !iban[..2].ToCharArray().All(char.IsLetter))
            {
                return false;
            }

            iban = iban.ToUpper();
            var changediban = iban.Substring(4, 22);
            changediban = changediban.Insert(22, (Convert.ToInt16(iban[0]) - 55).ToString());
            changediban = changediban.Insert(24, (Convert.ToInt16(iban[1]) - 55).ToString());
            changediban = changediban.Insert(26, iban.Substring(2, 2));
            if (decimal.Parse(changediban) % 97 != 1)
            {
                return false;
            }

            return true;
        }

        public static string? NormalizeMobile(this string? mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return string.Empty;
            }

            if (mobile.StartsWith("09"))
            {
                return $"+98{mobile.TrimStart('0')}";
            }

            if (mobile.StartsWith("009"))
            {
                return $"+{mobile.TrimStart('0')}";
            }

            if (mobile.StartsWith("9"))
            {
                return $"+98{mobile}";
            }

            return mobile;
        }

        public static string? NormalizeIranianMobile(this string? mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return string.Empty;
            }

            if (mobile.StartsWith("0098"))
            {
                return mobile[4..].PadLeft(11, '0');
            }

            if (mobile.StartsWith("+98"))
            {
                return mobile[3..].PadLeft(11, '0');
            }

            if (mobile.StartsWith("989"))
            {
                return mobile[2..].PadLeft(11, '0');
            }

            return mobile.PadLeft(11, '0');
        }

        public static HttpClient CreateHttpClient(this IHttpClientFactory httpClientFactory, bool forceTls13 = false)
        {
            return httpClientFactory.CreateClient(forceTls13 ? Constants.HttpClientIgnoreSslAndAutoRedirectTls13 : Constants.HttpClientIgnoreSslAndAutoRedirect);
        }

        public static dynamic GetAllResourceString(string defaultNamespace)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()?.FirstOrDefault(t => t.FullName?.Contains($"{defaultNamespace}.Resource") == true);
            if (assembly == null)
            {
                return string.Empty;
            }

            var names = assembly.GetManifestResourceNames();
            string[] validPrefixes =
            [
                $"{defaultNamespace}.Resource.UI.Web.Api.",
                    $"{defaultNamespace}.Resource.Data.ViewModel.",
                    $"{defaultNamespace}.Resource.Data.Enumeration.",
                ];
            dynamic expando = new ExpandoObject();
            Dictionary<string, object?> dataList = [];
            for (int i = 0; i < names.Length; i++)
            {
                string? item = names[i];
                if (validPrefixes.Exists(item.StartsWith) is false)
                {
                    continue;
                }

                using var cultureResourceStream = assembly.GetManifestResourceStream(item);
                if (cultureResourceStream is null)
                {
                    continue;
                }

                var baseName = item.TrimEnd(".resources");
                var manager = new ResourceManager(baseName, assembly);
                using var resources = new ResourceReader(cultureResourceStream);
                Dictionary<string, object?> nestedList = [];
                foreach (DictionaryEntry entry in resources)
                {
                    var key = (string)entry.Key;
                    var text = manager.GetString(key, CultureInfo.CurrentCulture);

                    var lst = key.Replace("_Name", string.Empty).Split("_", 2, StringSplitOptions.RemoveEmptyEntries);
                    if (lst.Length == 1)
                    {
                        nestedList.Add(lst[0], text);
                    }
                    else
                    {
                        if (nestedList.ContainsKey(lst[0]))
                        {
                            _ = (nestedList[lst[0]] as Dictionary<string, object?>)?.TryAdd(lst[1], text);
                        }
                        else
                        {
                            nestedList.Add(lst[0], new Dictionary<string, object?> { { lst[1], text } });
                        }
                    }
                }

                dataList.Add(baseName.Replace($"{defaultNamespace}.Resource.", string.Empty), nestedList);
            }

            expando.Data = dataList;

            Dictionary<string, object?> coreList = [];
            var resourceSet = Resources.GlobalResource.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            if (resourceSet is not null)
            {
                foreach (DictionaryEntry item in resourceSet)
                {
                    var key = item.Key.ToString();
                    if (key is null)
                    {
                        continue;
                    }

                    var lst = key.Replace("_Name", string.Empty).Split("_", 2, StringSplitOptions.RemoveEmptyEntries);
                    if (lst.Length == 1)
                    {
                        coreList.Add(lst[0], item.Value);
                    }
                    else
                    {
                        if (coreList.ContainsKey(lst[0]))
                        {
                            _ = (coreList[lst[0]] as Dictionary<string, object?>)?.TryAdd(lst[1], item.Value);
                        }
                        else
                        {
                            coreList.Add(lst[0], new Dictionary<string, object?> { { lst[1], item.Value } });
                        }
                    }
                }
            }

            expando.Core = coreList;

            return expando;
        }

        public static dynamic ToPowerOf<T>(this int number, int powerOf)
            where T : INumber<T>
        {
            if (powerOf < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(powerOf));
            }

            if (powerOf == 0)
            {
                if (number == 0)
                {
                    throw new ArgumentException("Parameters number and powerOf cannot be 0 at the same time");
                }

                return 1;
            }

            dynamic result = number;
            for (int i = 2; i <= powerOf; i++)
            {
                result = result * number;
            }

            return result;
        }

        public static void LogException(this ILogger logger, Exception exc, [CallerMemberName] string? methodName = "")
        {
#pragma warning disable CA2254 // Template should be a static expression
            logger.LogError(exc, methodName);
#pragma warning restore CA2254 // Template should be a static expression
        }

        public static void LogMessage(this ILogger logger, LogLevel logLevel, string? message, [CallerMemberName] string? methodName = "")
        {
#pragma warning disable CA2254 // Template should be a static expression
            logger.Log(logLevel, message + " " + methodName);
#pragma warning restore CA2254 // Template should be a static expression
        }

        public static string GenerateRandomString(int length)
        {
            int numberOfNonAlphanumericCharacters = GenerateRandomNumber(0, length);
            char[] punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

            if (length is < 1 or > 128)
            {
                throw new ArgumentException($"{nameof(length)} parameter should be between 1 and 128");
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            using var rng = RandomNumberGenerator.Create();
            var byteBuffer = new byte[length];
            rng.GetBytes(byteBuffer);
            var count = 0;
            var characterBuffer = new char[length];

            for (var iter = 0; iter < length; iter++)
            {
                var i = byteBuffer[iter] % 87;

                if (i < 10)
                {
                    characterBuffer[iter] = (char)('0' + i);
                }
                else if (i < 36)
                {
                    characterBuffer[iter] = (char)('A' + i - 10);
                }
                else if (i < 62)
                {
                    characterBuffer[iter] = (char)('a' + i - 36);
                }
                else
                {
                    characterBuffer[iter] = punctuations[i - 62];
                    count = count + 1;
                }
            }

            if (count >= numberOfNonAlphanumericCharacters)
            {
                return new string(characterBuffer);
            }

            int j;

            for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
            {
                int k;
                do
                {
                    k = GenerateRandomNumber(0, length - 1);
                }
                while (!char.IsLetterOrDigit(characterBuffer[k]));

                characterBuffer[k] = punctuations[GenerateRandomNumber(0, punctuations.Length - 1)];
            }

            return new string(characterBuffer);

            static int GenerateRandomNumber(int minValue, int maxValue)
            {
                using var rng = RandomNumberGenerator.Create();
                byte[] randomNumber = new byte[1];
                rng.GetBytes(randomNumber);

                double doubleRandomNumber = Convert.ToDouble(randomNumber[0]);

                double multiplier = Math.Max(0, (doubleRandomNumber / 255d) - 0.00000000001d);

                int range = maxValue - minValue + 1;

                double randomValueInRange = Math.Floor(multiplier * range);

                return (int)(minValue + randomValueInRange);
            }
        }

        internal static string? PrepareResourcePath(this string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            const string resourceAssembly = ".Resource";

            var str = UiDotRegex().Replace(path, resourceAssembly + "$0");
            str = UiCommaRegex().Replace(str, resourceAssembly + ",");

            return str
                .Replace(".Infrastructure.", resourceAssembly + ".Infrastructure.")
                .Replace(".Infrastructure,", resourceAssembly + ",")

                .Replace(".DomainService.", resourceAssembly + ".DomainService.")
                .Replace(".DomainService,", resourceAssembly + ",")

                .Replace(".Shared.", resourceAssembly + ".Shared.")
                .Replace(".Shared,", resourceAssembly + ",")

                .Replace(".Data.", resourceAssembly + ".Data.")
                .Replace(".Data,", resourceAssembly + ",")

                .Replace(".Common.", resourceAssembly + ".Common.")
                .Replace(".Common,", resourceAssembly + ",")
                .Replace(resourceAssembly + resourceAssembly, resourceAssembly);
        }

        internal static string? GetLocalizedValueInternal(DisplayAttribute displayAttribute, string propertyName, Constants.ResourceKey resourceKey, ResourceManager? cachedResourceManager = null, MemberInfo? member = null)
        {
            var result = resourceKey switch
            {
                Constants.ResourceKey.ShortName => displayAttribute.ShortName,
                Constants.ResourceKey.Description => displayAttribute.Description,
                Constants.ResourceKey.Prompt => displayAttribute.Prompt,
                Constants.ResourceKey.GroupName => displayAttribute.GroupName,
                _ => displayAttribute.Name,
            };
            if (result is not null)
            {
                return result;
            }

            if (cachedResourceManager is null)
            {
                if (displayAttribute.ResourceType is not null)
                {
                    cachedResourceManager = new ResourceManager(displayAttribute.ResourceType);
                }
                else if (displayAttribute.ResourceTypeName is not null)
                {
                    var type = Type.GetType(displayAttribute.ResourceTypeName) ?? throw new ArgumentException(nameof(DisplayAttribute.ResourceTypeName));
                    cachedResourceManager = new ResourceManager(type);
                }
                else if (member is not null)
                {
                    var type = Type.GetType(member.DeclaringType?.AssemblyQualifiedName.PrepareResourcePath()!);
                    if (type is not null)
                    {
                        cachedResourceManager = new ResourceManager(type);
                    }
                }
            }

            if (cachedResourceManager is not null)
            {
                if (displayAttribute.EnumType is null)
                {
                    result = cachedResourceManager.GetString($"{propertyName}_{resourceKey}");
                }
                else
                {
                    result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}_{resourceKey}");

                    if (result.IsNullOrEmpty() && resourceKey == Constants.ResourceKey.Name)
                    {
                        result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}");
                    }
                }
            }

            /*else if (displayAttribute.ResourceTypeName.IsNullOrEmpty() is false)
            {
                if (cachedResourceManager is null)
                {
                    cachedResourceManager = new ResourceManager(Type.GetType(displayAttribute.ResourceTypeName));
                }

                if (displayAttribute.EnumType is null)
                {
                    result = cachedResourceManager.GetString($"{propertyName}_{resourceKey}");
                }
                else
                {
                    result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}_{resourceKey}");
                    if (resourceKey == Constants.ResourceKey.Name && string.IsNullOrEmpty(result))
                    {
                        result = cachedResourceManager.GetString($"{displayAttribute.EnumType.Name}_{propertyName}");
                    }
                }
            }*/

            return result;
        }

        internal static RouteValueDictionary PrepareValues(object? routeValues, string? area = null)
        {
            var rootValueDictionary = new RouteValueDictionary(routeValues);
            if (!rootValueDictionary.ContainsKey(Constants.LanguageIdentifier))
            {
                rootValueDictionary.Add(Constants.LanguageIdentifier, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            }
            else
            {
                rootValueDictionary[Constants.LanguageIdentifier] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }

            if (!string.IsNullOrEmpty(area))
            {
                if (!rootValueDictionary.ContainsKey(Constants.AreaIdentifier))
                {
                    rootValueDictionary.Add(Constants.AreaIdentifier, area);
                }
                else
                {
                    rootValueDictionary[Constants.AreaIdentifier] = area;
                }
            }

            return rootValueDictionary;
        }

        [GeneratedRegex(".UI..*?,")]
        private static partial Regex UiCommaRegex();

        [GeneratedRegex(".UI..*?.")]
        private static partial Regex UiDotRegex();

        #region Inner Classes

        private class MyPersianCalendar : PersianCalendar
        {
            public override int GetYear(DateTime time)
            {
                try
                {
                    return base.GetYear(time);
                }
                catch
                {
                    return time.Year;
                }
            }

            public override int GetMonth(DateTime time)
            {
                try
                {
                    return base.GetMonth(time);
                }
                catch
                {
                    return time.Month;
                }
            }

            public override int GetDayOfMonth(DateTime time)
            {
                try
                {
                    return base.GetDayOfMonth(time);
                }
                catch
                {
                    return time.Day;
                }
            }

            public override int GetDayOfYear(DateTime time)
            {
                try
                {
                    return base.GetDayOfYear(time);
                }
                catch
                {
                    return time.DayOfYear;
                }
            }

            public override System.DayOfWeek GetDayOfWeek(DateTime time)
            {
                try
                {
                    return base.GetDayOfWeek(time);
                }
                catch
                {
                    return time.DayOfWeek;
                }
            }
        }

        #endregion
    }
}

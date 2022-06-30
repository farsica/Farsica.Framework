namespace Farsica.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class EnumHelper
    {
        public static string LocalizeEnum(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var items = value.ToString().Split(',', StringSplitOptions.TrimEntries);
            return string.Join(",", items.Select(t => Globals.GetLocalizedDisplayName(value.GetType().GetField(t)) ?? t));
        }

        public static string LocalizeEnum<T>(object value)
            where T : struct
        {
            if (value == null)
            {
                return string.Empty;
            }

            var items = value.ToString().Split(',', StringSplitOptions.TrimEntries);
            return string.Join(",", items.Select(t => Globals.GetLocalizedDisplayName(typeof(T).GetField(t)) ?? t));
        }

        public static string LocalizedShortName<T>(T value)
            where T : struct
        {
            var items = value.ToString().Split(',', StringSplitOptions.TrimEntries);
            return string.Join(",", items.Select(t => Globals.GetLocalizedShortName(typeof(T).GetField(t)) ?? t));
        }

        public static string LocalizedDescription<T>(T value)
            where T : struct
        {
            var items = value.ToString().Split(',', StringSplitOptions.TrimEntries);
            return string.Join(",", items.Select(t => Globals.GetLocalizedDescription(typeof(T).GetField(t)) ?? t));
        }

        public static bool TryParse<T>(this string name, out T t)
            where T : struct
        {
            t = default;

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (Enum.TryParse(name, true, out t))
            {
                return true;
            }

            Type type = typeof(T);
            var fields = type.GetFields();
            foreach (var info in fields)
            {
                if (name == info.Name
                    || name == Globals.GetLocalizedDisplayName(info)
                    || name == Globals.GetLocalizedShortName(info))
                {
                    t = (T)info.GetValue(info);
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> FlagsEnumToList<T>(this T value)
            where T : struct
        {
            if (!typeof(T).IsSubclassOf(typeof(Enum)))
            {
                throw new ArgumentException("typeof(T)");
            }

            return value.ToString().Split(',').Select(flag => (T)Enum.Parse(typeof(T), flag));
        }

        public static T ListToFlagsEnum<T>(this IEnumerable<T> value)
            where T : struct, IConvertible
        {
            var intlist = value.Select(t => t.ToInt32(new NumberFormatInfo()));
            var aggregatedint = intlist.Aggregate((prev, next) => prev | next);

            return (T)Enum.ToObject(typeof(T), aggregatedint);
        }
    }
}

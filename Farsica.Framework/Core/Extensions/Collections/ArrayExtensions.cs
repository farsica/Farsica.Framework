namespace Farsica.Framework.Core.Extensions.Collections
{
    using System;

    public static class ArrayExtensions
    {
        public static int BinarySearch<TValue, TKey>(this TValue[] list, TKey key, Func<TValue, TKey, int> comparer)
        {
            ArgumentNullException.ThrowIfNull(list, nameof(list));
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));

            int low = 0;
            int high = list.Length - 1;
            while (low <= high)
            {
                var mid = low + ((high - low) >> 1);
                var order = comparer(list[mid], key);
                if (order == 0)
                {
                    return mid;
                }
                else if (order > 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return ~low;
        }

        public static int BinarySearchFirst<TValue, TKey>(this TValue[] list, TKey key, Func<TValue, TKey, int> comparer)
        {
            int index = list.BinarySearch(key, comparer);
            for (; index > 0 && comparer(list[index - 1], key) == 0; index--)
            {
            }

            return index;
        }

        public static T? Find<T>(this T[] list, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match, nameof(match));

            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i]))
                {
                    return list[i];
                }
            }

            return default;
        }

        public static bool Exists<T>(this T[] list, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match, nameof(match));

            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

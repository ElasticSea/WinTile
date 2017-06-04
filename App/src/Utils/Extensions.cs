using System;
using System.Collections.Generic;

namespace App
{
    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        public static int? ToInt(this string s, int? def = (int?) null)
        {
            int i;
            return Int32.TryParse(s, out i) ? i : def;
        }

        public static float? ToFloat(this string s, float? def = (int?)null)
        {
            float i;
            return float.TryParse(s, out i) ? i : def;
        }

        public static void let<T>(this T item, Action<T> action)
        {
            action?.Invoke(item);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows;

namespace App.Utils
{
    internal static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
                action(item);
        }

        public static int? ToInt(this string s, int? def = (int?) null)
        {
            int i;
            return int.TryParse(s, out i) ? i : def;
        }

        public static float? ToFloat(this string s, float? def = (int?)null)
        {
            float i;
            return float.TryParse(s, out i) ? i : def;
        }

        public static double? ToDouble(this string s, double? def = (int?)null)
        {
            double i;
            return double.TryParse(s, out i) ? i : def;
        }

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static double Clamp(this float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static double Clamp(this double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static Vector Normalized(this Vector vector)
        {
            vector.Normalize();
            return vector;
        }

        public static Vector Multiply(this Vector a, Vector b)
        {
            return new Vector(a.X * b.X, a.Y * b.Y);
        }
    }
}
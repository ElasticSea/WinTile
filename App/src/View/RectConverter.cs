using System;
using System.Globalization;
using System.Windows.Data;

namespace App
{
    [ValueConversion(typeof(object), typeof(string))]
    public class Multiplier : IValueConverter
    {
        private readonly float multiplier;

        public Multiplier(float multiplier)
        {
            this.multiplier = multiplier;
        }

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (int)Math.Round((int)value * multiplier);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (value as string).ToInt(0) / multiplier;
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;
using App.Utils;

namespace App.View.Convertors
{
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return Math.Round(((double)value).Clamp(0, 1) * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (((string)value).ToDouble(0).Value / 100).Clamp(0, 1);
        }
    }
}
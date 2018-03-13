using System;
using System.Globalization;
using System.Windows.Data;

namespace App.View.Convertors
{
    [ValueConversion(typeof(float), typeof(string))]
    public class PercentConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return Math.Round(((float)value).Clamp(0, 1) * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (((string)value).ToFloat(0).Value / 100).Clamp(0, 1);
        }
    }
}
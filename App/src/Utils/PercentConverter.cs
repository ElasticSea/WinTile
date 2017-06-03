using System;
using System.Globalization;
using System.Windows.Data;

namespace App
{
    [ValueConversion(typeof(object), typeof(string))]
    public class PercentConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (int)((float)value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return float.Parse(value.ToString()) / 100;
        }
    }
}
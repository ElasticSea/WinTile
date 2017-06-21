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
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");

            return (int)((float)value *100);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
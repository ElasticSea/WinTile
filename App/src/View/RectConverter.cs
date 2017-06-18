using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace App
{
    [ValueConversion(typeof(object), typeof(string))]
    public class Multiplier : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (int)value * (parameter as string).ToFloat();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (value as string).ToFloat(0) / (parameter as string).ToFloat();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
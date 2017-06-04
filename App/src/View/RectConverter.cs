using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace App
{
    [ValueConversion(typeof(object), typeof(string))]
    public class Multiplier : MarkupExtension, IValueConverter
    {
        public float Multiplier1 { get; }

        public Multiplier(float multiplier)
        {
            this.Multiplier1 = multiplier;
        }

        public Multiplier()
        {
        }

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (int)Math.Round((int)value * Multiplier1);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (value as string).ToInt(0) / Multiplier1;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
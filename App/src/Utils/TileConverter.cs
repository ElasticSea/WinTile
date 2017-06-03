using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using App.Model;

namespace App
{
    [ValueConversion(typeof(object), typeof(string))]
    public class TileConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return (value as ObservableCollection<Tile>).Select(t => "ahoj");
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return value;
        }
    }
}
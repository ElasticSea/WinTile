using System.Windows.Media;
using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class Window
    {
        public bool Selected { get; set; }
        public SolidColorBrush Bursh { get; set; }
        public Rect Rect { get; set; }

        public Window(bool selected, SolidColorBrush bursh, Rect rect)
        {
            Selected = selected;
            Bursh = bursh;
            Rect = rect;
        }
    }
}
using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class Window
    {
        public bool Selected { get; set; }
        public Rect Rect { get; set; }

        public Window(Rect rect)
        {
            Rect = rect;
        }
    }
}
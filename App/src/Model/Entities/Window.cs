using PropertyChanged;

namespace ElasticSea.Wintile.Model.Entities
{
    [ImplementPropertyChanged]
    public class Window
    {
        public Window(Rect rect)
        {
            Rect = rect;
        }

        public bool Selected { get; set; }
        public Rect Rect { get; set; }
    }
}
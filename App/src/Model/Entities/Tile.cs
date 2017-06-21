using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Tile
    {
        public Tile(Rect rect)
        {
            Rect = rect;
        }

        public Rect Rect { get; set; }
    }
}
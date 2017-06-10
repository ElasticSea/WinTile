using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Tile
    {
        public Rect Rect { get; }

        public Tile(Rect rect)
        {
            Rect = rect;
        }

        public override string ToString()
        {
            return $"{nameof(Rect)}: {Rect}";
        }
    }
}
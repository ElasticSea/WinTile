namespace App.Model
{
    public class Tile
    {
        public Rect Rect { get; }
        public Hotkey Hotkey { get; }

        public Tile(Rect rect, Hotkey hotkey = null)
        {
            Rect = rect;
            Hotkey = hotkey;
        }

        public override string ToString()
        {
            return $"{nameof(Rect)}: {Rect}, {nameof(Hotkey)}: {Hotkey}";
        }
    }
}
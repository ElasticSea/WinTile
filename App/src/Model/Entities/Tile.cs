namespace App.Model
{
    public class Tile
    {
        public Rect rect;
        public Hotkey hotkey;

        public Tile(Rect rect, Hotkey hotkey = null)
        {
            this.rect = rect;
            this.hotkey = hotkey;
        }

        public override string ToString()
        {
            return $"{nameof(rect)}: {rect}, {nameof(hotkey)}: {hotkey}";
        }
    }
}
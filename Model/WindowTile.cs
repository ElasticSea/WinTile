using System.Windows.Input;

namespace WinTile.Model
{
    public class WindowTile
    {
        public Rect tile;
        public Key hotkey;

        public WindowTile(Rect tile, Key hotkey = Key.None)
        {
            this.tile = tile;
            this.hotkey = hotkey;
        }
    }
}
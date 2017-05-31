namespace WinTile.Model
{
    public class WindowTile
    {
        public Rect tile;
        private string hotkey;

        public WindowTile(Rect tile, string hotkey = null)
        {
            this.tile = tile;
            this.hotkey = hotkey;
        }
    }
}
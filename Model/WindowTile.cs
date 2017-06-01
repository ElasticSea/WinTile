namespace WinTile.Model
{
    public class WindowTile
    {
        public Rect rect;
        public Hotkey hotkey;

        public WindowTile(Rect rect, Hotkey hotkey = null)
        {
            this.rect = rect;
            this.hotkey = hotkey;
        }
    }
}
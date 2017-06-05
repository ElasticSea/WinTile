using App.Model;
using static System.Windows.SystemParameters;

namespace App
{
    public class WindowsTileSystem : ITilePositionManager
    {
        public void PositionTile(Tile tile)
        {
            var pxRect = tile.Rect.extend((int)WorkArea.Width, (int)WorkArea.Height) / 100;
            User32Utils.SetCurrentWindowPos(pxRect.Left, pxRect.Top, pxRect.Width, pxRect.Height);
        }
    }
}
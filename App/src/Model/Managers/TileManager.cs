using System.Collections.ObjectModel;
using static System.Windows.SystemParameters;

namespace App.Model.Managers
{
    public class TileManager
    {
        public Tile Selected;

        private readonly ObservableCollection<Tile> tiles;

        public TileManager(ObservableCollection<Tile> tiles)
        {
            this.tiles = tiles;
        }

        private Tile NextInDirection(int dir) => tiles[(tiles.IndexOf(Selected) + dir + tiles.Count) % tiles.Count];

        public void PositionWindow(Tile tile)
        {
            var pxRect = tile.Rect.extend((int)WorkArea.Width, (int)WorkArea.Height) / 100;
            User32Utils.SetCurrentWindowPos(pxRect.Left, pxRect.Top, pxRect.Width, pxRect.Height);
        }

        public void PositionPrev()
        {
            Selected = NextInDirection(-1);
            PositionWindow(Selected);
        }

        public void PositionNext()
        {
            Selected = NextInDirection(+1);
            PositionWindow(Selected);
        }
    }
}
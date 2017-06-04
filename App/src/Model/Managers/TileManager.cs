using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static System.Windows.SystemParameters;

namespace App.Model.Managers
{
    public class TileManager
    {
        private readonly TilePositionManager manager;

        private readonly ObservableCollection<Tile> tiles;
        public Tile Selected;

        public TileManager(ObservableCollection<Tile> tiles, TilePositionManager manager)
        {
            this.tiles = tiles;
            this.manager = manager;
        }

        private Tile NextInDirection(int dir)
        {
            return tiles[(tiles.IndexOf(Selected) + dir + tiles.Count) % tiles.Count];
        }

        public void PositionWindow(Tile tile)
        {
            var pxRect = tile.Rect.extend((int) WorkArea.Width, (int) WorkArea.Height) / 100;
            manager.MoveCurrentWindow(pxRect.Left, pxRect.Top, pxRect.Width, pxRect.Height);
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

        public void PositionClosestRight()
        {
            tiles
                .Where(t => t.Rect.Left >= Selected.Rect.Right)
                .OrderBy(t => t.Rect.Left)
                .FirstOrDefault()?.let(s => Selected = s);
            PositionWindow(Selected);
        }

        public void PositionClosestLeft()
        {
            tiles
                .Where(t => t.Rect.Right >= Selected.Rect.Left)
                .GroupBy(t => t.Rect.Right)
                .FirstOrDefault()?.let(s => Selected = s);
            PositionWindow(Selected);
        }

        public void PositionClosestUp()
        {
            tiles
                .Where(t => t.Rect.Bottom >= Selected.Rect.Top)
                .OrderBy(t => t.Rect.Bottom)
                .FirstOrDefault()?.let(s => Selected = s);
            PositionWindow(Selected);
        }

        public void PositionClosestDown()
        {
            tiles
                .Where(t => t.Rect.Top >= Selected.Rect.Bottom)
                .OrderBy(t => t.Rect.Top)
                .FirstOrDefault()?.let(s => Selected = s);
            PositionWindow(Selected);
        }

        public float TitleCloserating(Vector direction, Tile original, Tile target)
        {
            var ocx = original.Rect.Cx;
            var ocy = original.Rect.Cy;

            var tcx = target.Rect.Cx;
            var tcy = target.Rect.Cy;

            var ovec = new Vector(ocx, ocy);
            var tvec = new Vector(tcx, tcy);
            var vec = tvec - ovec;

            ovec.Normalize();
            tvec.Normalize();
            vec.Normalize();

            var dx = tcx - ocx;
            var dy = tcy - ocy;


            var dot = direction * vec;
            var angle = Math.Acos(dot);

            var limit = Math.PI / 2;
            var dirHeur = (limit - angle) / limit;
            var distHeur = 1 / Math.Sqrt(dx * dx + dy * dy);
            return (float) (dirHeur * distHeur);
        }
    }
}
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
            GetClosest(new Vector(1, 0))
                ?.@let(s =>
                {
                    Selected = s;
                    PositionWindow(Selected);
                });
        }

        public void PositionClosestLeft()
        {
            GetClosest(new Vector(-1, 0))
                ?.@let(s =>
                {
                    Selected = s;
                    PositionWindow(Selected);
                });
        }

        public void PositionClosestUp()
        {
            GetClosest(new Vector(0, -1))
                ?.@let(s =>
                {
                    Selected = s;
                    PositionWindow(Selected);
                });
        }

        public void PositionClosestDown()
        {
            GetClosest(new Vector(0, 1))
                ?.@let(s =>
                {
                    Selected = s;
                    PositionWindow(Selected);
                });
        }

        private Tile GetClosest(Vector direction)
        {
            return tiles
                .Select(t => new { Title = t, Penalty = TilePenalty(direction, Selected, t) })
                .OrderByDescending(a => a.Penalty)
                .FirstOrDefault(a => a.Penalty > 0)?.Title;
        }

        public static float TilePenalty(Vector direction, Tile original, Tile target)
        {
            var ocx = original.Rect.Cx;
            var ocy = original.Rect.Cy;

            var tcx = target.Rect.Cx;
            var tcy = target.Rect.Cy;

            var ovec = new Vector(ocx, ocy);
            var tvec = new Vector(tcx, tcy);
            var vec = tvec - ovec;
            vec.Normalize();

            var ocor = TileCorners(original);
            var tcor = TileCorners(target);

            var dist = Double.MaxValue;
            foreach (var oc in ocor)
            {
                foreach (var tc in tcor)
                {
                    var length = (tc - oc).Length;
                    if (length < dist)
                        dist = length;
                }
            }

            var dot = direction * vec;
            var angle = Math.Acos(dot);

            var limit = Math.PI / 2;
            var dirHeur = (limit - angle) / limit;
            var distHeur = 1 / (dist + 1);
            return (float) (dirHeur * distHeur);
        }

        private static Vector[] TileCorners(Tile tile) => new[]
        {
            new Vector(tile.Rect.Left, tile.Rect.Bottom),
            new Vector(tile.Rect.Right, tile.Rect.Bottom),
            new Vector(tile.Rect.Right, tile.Rect.Top),
            new Vector(tile.Rect.Left, tile.Rect.Top)
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class ClosestStrategy : PositioningStrategy
    {
        private static readonly Vector left = new Vector(-1, 0);
        private static readonly Vector right = new Vector(1, 0);
        private static readonly Vector up = new Vector(0, -1);
        private static readonly Vector down = new Vector(0, 1);

        public ClosestStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        public void Left() => GetClosest(left);
        public void Right() => GetClosest(right);
        public void Up() => GetClosest(up);
        public void Down() => GetClosest(down);

        private void GetClosest(Vector direction)
        {
            var tile =  tiles
                .Select(t => new { Title = t, Penalty = TilePenalty(direction, Selected, t) })
                .OrderByDescending(a => a.Penalty)
                .FirstOrDefault(a => a.Penalty > 0)?.Title;

            if (tile != null)
            {
                Selected = tile;
                windowManager.CurrentWindowRect = tile.Rect;
            }
        }

        private static float TilePenalty(Vector direction, Tile original, Tile target)
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
            return (float)(dirHeur * distHeur);
        }

        private static Vector[] TileCorners(Tile tile) => new[]
        {
            new Vector(tile.Rect.Left, tile.Rect.Bottom),
            new Vector(tile.Rect.Right, tile.Rect.Bottom),
            new Vector(tile.Rect.Right, tile.Rect.Top),
            new Vector(tile.Rect.Left, tile.Rect.Top)
        };

//        private Tile Closest(Rect rect) => tiles.OrderBy(
//            t => Math.Abs(t.Rect.Left - rect.Left) +
//                 Math.Abs(t.Rect.Right - rect.Right) +
//                 Math.Abs(t.Rect.Top - rect.Top) +
//                 Math.Abs(t.Rect.Bottom - rect.Bottom)
//        ).First();
    }
}
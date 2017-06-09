using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace App.Model.Managers
{
    public class ClosestStrategy : IPositioningStrategy
    {
        private static readonly Vector left = new Vector(-1, 0);
        private static readonly Vector right = new Vector(1, 0);
        private static readonly Vector up = new Vector(0, -1);
        private static readonly Vector down = new Vector(0, 1);

        private readonly IEnumerable<Tile> tiles;

        public ClosestStrategy(IEnumerable<Tile> tiles)
        {
            this.tiles = tiles;
        }

        public Tile Left(Tile selected) => GetClosest(left, selected);
        public Tile Right(Tile slected) => GetClosest(right, slected);
        public Tile Up(Tile slected) => GetClosest(up, slected);
        public Tile Down(Tile slected) => GetClosest(down, slected);

        private Tile GetClosest(Vector direction, Tile selected)
        {
            return tiles
                .Select(t => new { Title = t, Penalty = TilePenalty(direction, selected, t) })
                .OrderByDescending(a => a.Penalty)
                .FirstOrDefault(a => a.Penalty > 0)?.Title;
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
    }
}
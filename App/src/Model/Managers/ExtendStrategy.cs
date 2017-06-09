using System.Collections.Generic;
using System.Linq;

namespace App.Model.Managers
{
    public class ExtendStrategy : IPositioningStrategy
    {
        private readonly IEnumerable<Tile> tiles;

        public ExtendStrategy(IEnumerable<Tile> tiles)
        {
            this.tiles = tiles;
        }

        public Tile Left(Tile selected)
        {
            var candidates = tiles
                .Select(t => t.Rect.Left)
                .Where(l => l < selected.Rect.Left)
                .OrderByDescending(t => t);

            var left = candidates.Any() ? candidates.First() : (int?) null;

            var r = selected.Rect;
            var rect = new Rect(left ?? r.Left, r.Top, r.Right, r.Bottom);
            return tiles.FirstOrDefault(t => Equals(t.Rect, rect)) ?? new Tile(rect);
        }

        public Tile Right(Tile selected)
        {
            var candidates = tiles
                .Select(t => t.Rect.Right)
                .Where(l => l > selected.Rect.Right)
                .OrderBy(t => t);

            var right = candidates.Any() ? candidates.First() : (int?) null;

            var r = selected.Rect;
            var rect = new Rect(r.Left, r.Top, right ?? r.Right, r.Bottom);
            return tiles.FirstOrDefault(t => Equals(t.Rect, rect)) ?? new Tile(rect);
        }

        public Tile Up(Tile selected)
        {
            var candidates = tiles
                .Select(t => t.Rect.Top)
                .Where(l => l < selected.Rect.Top)
                .OrderByDescending(t => t);

            var top = candidates.Any() ? candidates.First() : (int?) null;

            var r = selected.Rect;
            var rect = new Rect(r.Left, top ?? r.Top, r.Right, r.Bottom);
            return tiles.FirstOrDefault(t => Equals(t.Rect, rect)) ?? new Tile(rect);
        }

        public Tile Down(Tile selected)
        {
            var candidates = tiles
                .Select(t => t.Rect.Bottom)
                .Where(l => l > selected.Rect.Bottom)
                .OrderBy(t => t);

            var bottom = candidates.Any() ? candidates.First() : (int?) null;

            var r = selected.Rect;
            var rect = new Rect(r.Left, r.Top, r.Right, bottom ?? r.Bottom);
            return tiles.FirstOrDefault(t => Equals(t.Rect, rect)) ?? new Tile(rect);
        }
    }
}
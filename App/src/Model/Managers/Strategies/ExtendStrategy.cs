using System.Collections.Generic;
using System.Linq;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class ExtendStrategy : PositioningStrategy
    {
        public ExtendStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        public void Left()
        {
            var candidates = tiles
                .Select(t => t.Rect.Left)
                .Where(l => l < Selected.Rect.Left)
                .OrderByDescending(t => t);

            var left = candidates.Any() ? candidates.First() : (int?) null;

            var r = Selected.Rect;
            var rect = new Rect(left ?? r.Left, r.Top, r.Right, r.Bottom);
            Position(rect);
        }

        public void Right()
        {
            var candidates = tiles
                .Select(t => t.Rect.Right)
                .Where(l => l > Selected.Rect.Right)
                .OrderBy(t => t);

            var right = candidates.Any() ? candidates.First() : (int?) null;

            var r = Selected.Rect;
            var rect = new Rect(r.Left, r.Top, right ?? r.Right, r.Bottom);
            Position(rect);
        }

        public void Up()
        {
            var candidates = tiles
                .Select(t => t.Rect.Top)
                .Where(l => l < Selected.Rect.Top)
                .OrderByDescending(t => t);

            var top = candidates.Any() ? candidates.First() : (int?) null;

            var r = Selected.Rect;
            var rect = new Rect(r.Left, top ?? r.Top, r.Right, r.Bottom);
            Position(rect);
        }

        public void Down()
        {
            var candidates = tiles
                .Select(t => t.Rect.Bottom)
                .Where(l => l > Selected.Rect.Bottom)
                .OrderBy(t => t);

            var bottom = candidates.Any() ? candidates.First() : (int?) null;

            var r = Selected.Rect;
            var rect = new Rect(r.Left, r.Top, r.Right, bottom ?? r.Bottom);
            Position(rect);
        }

        private void Position(Rect rect)
        {
            Selected = tiles.FirstOrDefault(t => Equals(t.Rect, rect)) ?? new Tile(rect);
            windowManager.PositionWindow(windowManager.FocusedWindow, Selected.Rect);
        }
    }
}
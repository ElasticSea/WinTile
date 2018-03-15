using System.Collections.Generic;
using System.Linq;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public class ExtendStrategy : PositioningStrategy
    {
        public ExtendStrategy(IList<Tile> tiles, IWindowManager windowManager) : base(tiles, windowManager)
        {
        }

        public void Left()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = tiles
                .Select(t => t.Rect.Left)
                .Where(l => l < Selected.Left)
                .OrderByDescending(t => t);

            var left = candidates.Any() ? candidates.First() : (float?) null;

            var r = Selected;
            var rect = new Rect(left ?? r.Left, r.Top, r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Right()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = tiles
                .Select(t => t.Rect.Right)
                .Where(l => l > Selected.Right)
                .OrderBy(t => t);

            var right = candidates.Any() ? candidates.First() : (float?) null;

            var r = Selected;
            var rect = new Rect(r.Left, r.Top, right ?? r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Up()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = tiles
                .Select(t => t.Rect.Top)
                .Where(l => l < Selected.Top)
                .OrderByDescending(t => t);

            var top = candidates.Any() ? candidates.First() : (float?) null;

            var r = Selected;
            var rect = new Rect(r.Left, top ?? r.Top, r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Down()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = tiles
                .Select(t => t.Rect.Bottom)
                .Where(l => l > Selected.Bottom)
                .OrderBy(t => t);

            var bottom = candidates.Any() ? candidates.First() : (float?) null;

            var r = Selected;
            var rect = new Rect(r.Left, r.Top, r.Right, bottom ?? r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }
    }
}
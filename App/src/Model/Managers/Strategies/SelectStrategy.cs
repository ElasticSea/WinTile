using System;
using System.Collections.Generic;
using System.Linq;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class SelectStrategy : AbstractClosestStrategy
    {
        public SelectStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        protected override void OnClosestTIle(Tile tile)
        {
            var allwin = windowManager.GetVisibleWindows()
                .Select(t => new { Rect = windowManager.GetWindowRect(t), Handle = t });

            var closestWindowToRect = allwin.OrderBy(
                t => Math.Abs(t.Rect.Left - tile.Rect.Left) +
                     Math.Abs(t.Rect.Right - tile.Rect.Right) +
                     Math.Abs(t.Rect.Top - tile.Rect.Top) +
                     Math.Abs(t.Rect.Bottom - tile.Rect.Bottom)
            ).First();

            windowManager.FocusedWindow = closestWindowToRect.Handle;
        }
    }
}
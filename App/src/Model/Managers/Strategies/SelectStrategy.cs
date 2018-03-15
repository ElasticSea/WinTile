using System;
using System.Collections.Generic;
using System.Linq;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public class SelectStrategy : AbstractClosestStrategy
    {
        public SelectStrategy(IList<Rect> rects, IWindowManager windowManager) : base(rects, windowManager)
        {
        }

        protected override void OnClosestTIle(Rect tile)
        {
            var allwin = windowManager.GetVisibleWindows()
                .Select(t => new {Rect = windowManager.GetWindowRect(t), Handle = t});

            var closestWindowToRect = allwin.OrderBy(
                t => Math.Abs(t.Rect.Left - tile.Left) +
                     Math.Abs(t.Rect.Right - tile.Right) +
                     Math.Abs(t.Rect.Top - tile.Top) +
                     Math.Abs(t.Rect.Bottom - tile.Bottom)
            ).First();

            windowManager.FocusedWindow = closestWindowToRect.Handle;
        }
    }
}
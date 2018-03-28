using System.Collections.Generic;
using System.Windows;
using App.Model.Managers.Window;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Strategies
{
    public class MoveStrategy : ClosesRectToWindow
    {
        private readonly IEnumerable<Rect> tiles;

        public MoveStrategy(IEnumerable<Rect> tiles, IWindowManager windowManager) : base(windowManager)
        {
            this.tiles = tiles;
        }

        public void Left() => Move(left);
        public void Right() => Move(right);
        public void Up() => Move(up);
        public void Down() => Move(down);

        private void Move(Vector dir)
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);

            var rect = GetClosest(tiles, windowRect, dir);
            if (rect != null)
            {
                windowManager.PositionWindow(windowManager.FocusedWindow.Value, rect);
            }
            else
            {
                var cent = windowRect.Center;
                windowRect.Size = new Vector(0,0);
                windowRect.Center = cent;

                var rect2 = GetClosest(tiles, windowRect, dir);
                if (rect2 != null)
                {
                    windowManager.PositionWindow(windowManager.FocusedWindow.Value, rect2);
                }
            }
        }
    }
}
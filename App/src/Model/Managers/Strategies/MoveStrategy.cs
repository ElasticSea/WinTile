using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElasticSea.Wintile.Model.Managers.Window;
using Rect = ElasticSea.Wintile.Model.Entities.Rect;

namespace ElasticSea.Wintile.Model.Managers.Strategies
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
            else if (tiles.Contains(windowRect) == false)
            {
                var cent = windowRect.Center;
                windowRect.Size = new Vector(0, 0);
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
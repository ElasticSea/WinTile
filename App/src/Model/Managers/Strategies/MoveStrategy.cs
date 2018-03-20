using System.Collections.Generic;
using System.Linq;
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

        public void Left()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            ProcessRect(GetClosest(tiles, windowRect, left));
        }

        public void Right()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            ProcessRect(GetClosest(tiles, windowRect, right));
        }

        public void Up()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            ProcessRect(GetClosest(tiles, windowRect, up));
        }

        public void Down()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            ProcessRect(GetClosest(tiles, windowRect, down));
        }

        private void ProcessRect(Rect rect)
        {
            if (rect != null)
            {
                windowManager.PositionWindow(windowManager.FocusedWindow.Value, rect);
            }
            else
            {
                var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);

                if (GetClosest(tiles, windowRect, up) == null &&
                    GetClosest(tiles, windowRect, left) == null &&
                    GetClosest(tiles, windowRect, right) == null &&
                    GetClosest(tiles, windowRect, down) == null)
                {
                    windowManager.PositionWindow(windowManager.FocusedWindow.Value, tiles.First());
                }
            }
        }
    }
}
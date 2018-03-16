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
            ProcessRect(GetClosest(tiles, left));
        }

        public void Right()
        {
            ProcessRect(GetClosest(tiles, right));
        }

        public void Up()
        {
            ProcessRect(GetClosest(tiles, up));
        }

        public void Down()
        {
            ProcessRect(GetClosest(tiles, down));
        }

        private void ProcessRect(Rect rect)
        {
            if (rect != null)
            {
                windowManager.PositionWindow(windowManager.FocusedWindow, rect);
            }
            else
            {
                if (GetClosest(tiles, up) == null &&
                    GetClosest(tiles, left) == null &&
                    GetClosest(tiles, right) == null &&
                    GetClosest(tiles, down) == null)
                {
                    windowManager.PositionWindow(windowManager.FocusedWindow, tiles.First());
                }
            }
        }
    }
}
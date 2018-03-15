using System.Collections.Generic;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public class MoveStrategy : AbstractClosestStrategy
    {
        public MoveStrategy(IList<Rect> rects, IWindowManager windowManager) : base(rects, windowManager)
        {
        }

        protected override void OnClosestTIle(Rect tile)
        {
            windowManager.PositionWindow(windowManager.FocusedWindow, tile);
        }
    }
}
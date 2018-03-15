using System.Collections.Generic;
using App.Model.Managers.Window;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Strategies
{
    public class MoveStrategy : ClosesRectToWindow
    {
        public MoveStrategy(IEnumerable<Rect> tiles, IWindowManager windowManager) : base(windowManager)
        {
            Rects = tiles;
        }

        protected override void ProcessRect(Rect rect)
        {
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        protected override IEnumerable<Rect> Rects { get; }
    }
}
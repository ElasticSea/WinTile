using System.Collections.Generic;
using System.Linq;
using App.Model.Managers.Window;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Strategies
{
    public class SelectStrategy : ClosesRectToWindow
    {
        public SelectStrategy(IWindowManager windowManager) : base(windowManager)
        {
        }

        protected override void ProcessRect(Rect rect)
        {
            windowManager.FocusedWindow =
                windowManager.GetVisibleWindows().First(handle => windowManager.GetWindowRect(handle) == rect);
        }

        protected override IEnumerable<Rect> Rects => windowManager.GetVisibleWindows()
            .Select(t => windowManager.GetWindowRect(t)).ToList();
    }
}
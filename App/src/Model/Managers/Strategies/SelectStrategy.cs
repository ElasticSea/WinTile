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

        public void Left()
        {
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, left));
        }

        public void Right()
        {
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, right));
        }

        public void Up()
        {
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, up));
        }

        public void Down()
        {
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, down));
        }

        private void ProcessRect(Rect rect)
        {
            if (rect != null)
                windowManager.FocusedWindow =
                windowManager.GetVisibleWindows().First(handle => windowManager.GetWindowRect(handle) == rect);
        }
    }
}
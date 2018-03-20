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
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, windowRect, left));
        }

        public void Right()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, windowRect, right));
        }

        public void Up()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, windowRect, up));
        }

        public void Down()
        {
            if (windowManager.FocusedWindow == null) return;

            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow.Value);
            var rects = windowManager.GetVisibleWindows().Select(t => windowManager.GetWindowRect(t)).ToList();
            ProcessRect(GetClosest(rects, windowRect, down));
        }

        private void ProcessRect(Rect rect)
        {
            if (rect != null)
                windowManager.FocusedWindow =
                windowManager.GetVisibleWindows().First(handle => windowManager.GetWindowRect(handle) == rect);
        }
    }
}
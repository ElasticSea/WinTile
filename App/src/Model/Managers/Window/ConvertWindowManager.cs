using System;
using System.Collections.Generic;
using App.Model;
using static System.Windows.SystemParameters;

namespace App
{
    public class ConvertWindowManager : IWindowManager
    {
        private readonly IWindowManager wrapped;

        public ConvertWindowManager(IWindowManager wrapped)
        {
            this.wrapped = wrapped;
        }

        private Rect MonitorRect => new Rect(0, 0, (int) WorkArea.Width, (int) WorkArea.Height);

        public IntPtr FocusedWindow
        {
            get => wrapped.FocusedWindow;
            set => wrapped.FocusedWindow = value;
        }

        public IEnumerable<IntPtr> GetVisibleWindows()
        {
            return wrapped.GetVisibleWindows();
        }

        public Rect GetWindowRect(IntPtr handle)
        {
            return PxtoPercent(wrapped.GetWindowRect(handle));
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            wrapped.PositionWindow(handle, PercentToPx(rect));
        }

        private Rect PxtoPercent(Rect rect)
        {
            return rect.shrink(MonitorRect.Width, MonitorRect.Height);
        }

        private Rect PercentToPx(Rect rect)
        {
            return rect.extend(MonitorRect.Width, MonitorRect.Height);
        }
    }
}
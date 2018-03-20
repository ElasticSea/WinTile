using System;
using System.Collections.Generic;
using System.Windows;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Window
{
    public class ConvertWindowManager : IWindowManager
    {
        private readonly IWindowManager wrapped;

        public ConvertWindowManager(IWindowManager wrapped)
        {
            this.wrapped = wrapped;
        }

        private Rect MonitorRect => new Rect(0, 0, (int) SystemParameters.WorkArea.Width, (int) SystemParameters.WorkArea.Height);

        public IntPtr? FocusedWindow
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
            return rect.Shrink(MonitorRect.Size);
        }

        private Rect PercentToPx(Rect rect)
        {
            return rect.Extend(MonitorRect.Size);
        }
    }
}
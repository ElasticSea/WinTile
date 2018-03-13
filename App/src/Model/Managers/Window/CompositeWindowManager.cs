using System;
using System.Collections.Generic;
using App.Model;

namespace App.Utils
{
    public class CompositeWindowManager : IWindowManager
    {
        public IWindowManager CurrentManager { get; set; }

        public IntPtr FocusedWindow
        {
            get => CurrentManager.FocusedWindow;
            set => CurrentManager.FocusedWindow = value;
        }

        public IEnumerable<IntPtr> GetVisibleWindows()
        {
            return CurrentManager.GetVisibleWindows();
        }

        public Rect GetWindowRect(IntPtr handle)
        {
            return CurrentManager.GetWindowRect(handle);
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            CurrentManager.PositionWindow(handle, rect);
        }
    }
}
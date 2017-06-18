using System;
using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App
{
    public class WindowManagerDummy : IWindowManager
    {
        private Rect rect = new Rect();

        public IntPtr getCurrentWindow()
        {
            return IntPtr.Zero;
        }

        public IntPtr FocusedWindow { get; set; }
        public IEnumerable<IntPtr> GetVisibleWindows() => new List<IntPtr>{ IntPtr.Zero };

        public Rect GetWindowRect(IntPtr handle)
        {
            return rect;
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            this.rect = rect;
        }

        public void Focus(IntPtr closesWqInd)
        {
            throw new NotImplementedException();
        }
    }
}
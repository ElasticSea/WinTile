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

        public IEnumerable<IntPtr> getVisibleWIndows() => Enumerable.Empty<IntPtr>();

        public Rect getRectForWindow(IntPtr handle)
        {
            return rect;
        }

        public void MoveWindow(IntPtr handle, Rect rect)
        {
            this.rect = rect;
        }
    }
}
using System;
using System.Collections.Generic;
using App.Model;

namespace App
{
    public class WindowManagerDummy : IWindowManager
    {
        public IntPtr getCurrentWindow()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IntPtr> getVisibleWIndows()
        {
            throw new NotImplementedException();
        }

        public Rect getRectForWindow(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        public void MoveWindow(IntPtr handle, Rect rect)
        {
            throw new NotImplementedException();
        }
    }
}
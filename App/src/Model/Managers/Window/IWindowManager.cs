using System;
using System.Collections.Generic;
using App.Model;

namespace App
{
    public interface IWindowManager
    {
        IntPtr FocusedWindow { get; set; }
        IEnumerable<IntPtr> GetVisibleWindows();
        Rect GetWindowRect(IntPtr handle);
        void PositionWindow(IntPtr handle, Rect rect);
    }
}
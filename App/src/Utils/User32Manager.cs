using System;
using System.Collections.Generic;
using Rect = App.Model.Rect;

namespace App.Utils
{
    public class User32Manager : IWindowManager
    {
        public IntPtr getCurrentWindow() => User32Utils.CurrentWindow();
        public IEnumerable<IntPtr> getVisibleWIndows() => User32Utils.VisibleWindows();
        public Rect getRectForWindow(IntPtr handle) => User32Utils.GetWindoRect(handle);
        public void MoveWindow(IntPtr handle, Rect rect) => User32Utils.moveWIndow(rect, handle);
    }
}
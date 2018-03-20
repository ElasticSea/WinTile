using System;
using System.Collections.Generic;
using App.Model.Entities;

namespace App.Model.Managers.Window
{
    public interface IWindowManager
    {
        IntPtr? FocusedWindow { get; set; }
        IEnumerable<IntPtr> GetVisibleWindows();
        Rect GetWindowRect(IntPtr handle);
        void PositionWindow(IntPtr handle, Rect rect);
    }
}
using System;
using System.Collections.Generic;
using App.Model;

namespace App
{
    public interface IWindowManager
    {
        IntPtr getCurrentWindow();
        IEnumerable<IntPtr> getVisibleWIndows();
        Rect getRectForWindow(IntPtr handle);
        void MoveWindow(IntPtr handle, Rect rect);
        void Focus(IntPtr closesWqInd);
    }
}
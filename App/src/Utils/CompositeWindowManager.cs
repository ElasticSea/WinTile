using System;
using System.Collections.Generic;
using App.Model;

namespace App.Utils
{
    public class CompositeWindowManager : IWindowManager
    {
        private readonly IWindowManager User32Manager;
        private readonly IWindowManager WindowManagerDummy;

        public bool Active;
                    
        public CompositeWindowManager(IWindowManager user32Manager, IWindowManager windowManagerDummy)
        {
            User32Manager = user32Manager;
            WindowManagerDummy = windowManagerDummy;
        }

        private IWindowManager CurrentManager => Active ? User32Manager : WindowManagerDummy;

        public IntPtr getCurrentWindow() => CurrentManager.getCurrentWindow();
        public IEnumerable<IntPtr> getVisibleWIndows() => CurrentManager.getVisibleWIndows();
        public Rect getRectForWindow(IntPtr handle) => CurrentManager.getRectForWindow(handle);
        public void MoveWindow(IntPtr handle, Rect rect) => CurrentManager.MoveWindow(handle, rect);
        public void Focus(IntPtr closesWqInd) => CurrentManager.Focus(closesWqInd);
    }
}
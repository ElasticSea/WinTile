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

        public IntPtr FocusedWindow
        {
            get => CurrentManager.FocusedWindow;
            set => CurrentManager.FocusedWindow = value;
        }

        public IEnumerable<IntPtr> GetVisibleWindows() => CurrentManager.GetVisibleWindows();
        public Rect GetWindowRect(IntPtr handle) => CurrentManager.GetWindowRect(handle);
        public void PositionWindow(IntPtr handle, Rect rect) => CurrentManager.PositionWindow(handle, rect);
    }
}
using System;
using System.Collections.Generic;
using ElasticSea.Wintile.Model.Entities;

namespace ElasticSea.Wintile.Model.Managers.Window
{
    public class CompositeWindowManager : IWindowManager
    {
        public IWindowManager CurrentManager { get; set; }

        public IntPtr? FocusedWindow
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
using System;
using System.Collections.Generic;
using System.Linq;
using App.Model.Entities;
using App.Model.Managers.Window;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class LayoutStrategy
    {
        protected readonly IList<Rect> rects;
        protected readonly IWindowManager windowManager;

        protected LayoutStrategy(IList<Rect> rects, IWindowManager windowManager)
        {
            this.rects = rects;
            this.windowManager = windowManager;
        }

        public void Left()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);
            var value = Selected.Left;
            Move(value, -5, rect => rect.Left, (rect, i) => rect.Left = i);
            Move(value, -5, rect => rect.Right, (rect, i) => rect.Right = i);
        }

        public void Right()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);
            var value = Selected.Right;
            Move(value, 5, rect => rect.Left, (rect, i) => rect.Left = i);
            Move(value, 5, rect => rect.Right, (rect, i) => rect.Right = i);
        }

        public void Up()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);
            var value = Selected.Top;
            Move(value, -5, rect => rect.Top, (rect, i) => rect.Top = i);
            Move(value, -5, rect => rect.Bottom, (rect, i) => rect.Bottom = i);
        }

        public void Down()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);
            var value = Selected.Bottom;
            Move(value, 5, rect => rect.Top, (rect, i) => rect.Top = i);
            Move(value, 5, rect => rect.Bottom, (rect, i) => rect.Bottom = i);
        }

        private void Move(float border, float amount, Func<Rect, float> get, Action<Rect, float> set)
        {
            var allwin = windowManager.GetVisibleWindows()
                .Select(t => new {Rect = windowManager.GetWindowRect(t), Handle = t});
            allwin.Where(a => get(a.Rect) == border).ForEach(a =>
            {
                set(a.Rect, (get(a.Rect) + amount).Clamp(0, 1));
                windowManager.PositionWindow(a.Handle, a.Rect);
            });
            rects.Where(t => get(t) == border).ForEach(t => set(t, (get(t) + amount).Clamp(0, 1)));
        }
    }
}
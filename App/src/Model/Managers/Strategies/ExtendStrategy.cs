﻿using System.Collections.Generic;
using System.Linq;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public class ExtendStrategy
    {
        protected readonly IList<Rect> rects;
        protected readonly IWindowManager windowManager;

        public ExtendStrategy(IList<Rect> rects, IWindowManager windowManager)
        {
            this.rects = rects;
            this.windowManager = windowManager;
        }

        public void Left()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = rects
                .Select(t => t.Left)
                .Where(l => l < Selected.Left)
                .OrderByDescending(t => t);

            var left = candidates.Any() ? candidates.First() : (double?) null;

            var r = Selected;
            var rect = new Rect(left ?? r.Left, r.Top, r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Right()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = rects
                .Select(t => t.Right)
                .Where(l => l > Selected.Right)
                .OrderBy(t => t);

            var right = candidates.Any() ? candidates.First() : (double?) null;

            var r = Selected;
            var rect = new Rect(r.Left, r.Top, right ?? r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Up()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = rects
                .Select(t => t.Top)
                .Where(l => l < Selected.Top)
                .OrderByDescending(t => t);

            var top = candidates.Any() ? candidates.First() : (double?) null;

            var r = Selected;
            var rect = new Rect(r.Left, top ?? r.Top, r.Right, r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }

        public void Down()
        {
            var Selected = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var candidates = rects
                .Select(t => t.Bottom)
                .Where(l => l > Selected.Bottom)
                .OrderBy(t => t);

            var bottom = candidates.Any() ? candidates.First() : (double?) null;

            var r = Selected;
            var rect = new Rect(r.Left, r.Top, r.Right, bottom ?? r.Bottom);
            windowManager.PositionWindow(windowManager.FocusedWindow, rect);
        }
    }
}
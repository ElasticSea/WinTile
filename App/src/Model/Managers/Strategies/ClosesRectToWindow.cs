﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using App.Model.Managers.Window;
using App.Utils;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Strategies
{
    public abstract class ClosesRectToWindow
    {
        private static readonly Vector left = new Vector(-1, 0);
        private static readonly Vector right = new Vector(1, 0);
        private static readonly Vector up = new Vector(0, -1);
        private static readonly Vector down = new Vector(0, 1);

        protected readonly IWindowManager windowManager;

        protected ClosesRectToWindow(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public void Left()
        {
            GetClosest(left);
        }

        public void Right()
        {
            GetClosest(right);
        }

        public void Up()
        {
            GetClosest(up);
        }

        public void Down()
        {
            GetClosest(down);
        }

        private void GetClosest(Vector direction)
        {
            var windowRect = windowManager.GetWindowRect(windowManager.FocusedWindow);

            var rect = Rects
                .Select(t => new { rect = t, Penalty = ComputePenalty(direction, windowRect, t)})
                .OrderByDescending(a => a.Penalty)
                .FirstOrDefault(a => a.Penalty > 0)?.rect;

            if (rect != null)
                ProcessRect(rect);
        }

        private static float ComputePenalty(Vector lookDirection, Rect original, Rect target)
        {
            var originalC = new Vector(original.Cx, original.Cy);
            var targetC = new Vector(target.Cx, target.Cy);
            var rectCenterVector = (targetC - originalC);

            var distance = rectCenterVector.Length;
            var direction = rectCenterVector.Normalized();

            var dot = lookDirection * direction;
            return (float)(dot * (1 / distance));
        }

        protected abstract void ProcessRect(Rect rect);

        protected abstract IEnumerable<Rect> Rects { get; }
    }
}
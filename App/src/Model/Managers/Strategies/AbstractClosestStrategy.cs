using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using App.Model.Entities;
using App.Model.Managers.Window;
using Rect = App.Model.Entities.Rect;

namespace App.Model.Managers.Strategies
{
    public abstract class AbstractClosestStrategy
    {
        private static readonly Vector left = new Vector(-1, 0);
        private static readonly Vector right = new Vector(1, 0);
        private static readonly Vector up = new Vector(0, -1);
        private static readonly Vector down = new Vector(0, 1);

        protected readonly IList<Rect> rects;
        protected readonly IWindowManager windowManager;

        protected AbstractClosestStrategy(IList<Rect> rects, IWindowManager windowManager)
        {
            this.rects = rects;
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
            var Selected = Closest(windowManager.GetWindowRect(windowManager.FocusedWindow));

            var tile = rects
                .Select(t => new {Title = t, Penalty = TilePenalty(direction, Selected, t)})
                .OrderByDescending(a => a.Penalty)
                .FirstOrDefault(a => a.Penalty > 0)?.Title;

            if (tile != null)
                OnClosestTIle(tile);
        }

        protected abstract void OnClosestTIle(Rect tile);

        private static float TilePenalty(Vector direction, Rect original, Rect target)
        {
            var ocx = original.Cx;
            var ocy = original.Cy;

            var tcx = target.Cx;
            var tcy = target.Cy;

            var ovec = new Vector(ocx, ocy);
            var tvec = new Vector(tcx, tcy);
            var vec = tvec - ovec;
            vec.Normalize();

            var ocor = RectCorners(original);
            var tcor = RectCorners(target);

            var dist = double.MaxValue;
            foreach (var oc in ocor)
            foreach (var tc in tcor)
            {
                var length = (tc - oc).Length;
                if (length < dist)
                    dist = length;
            }

            var dot = direction * vec;
            var angle = Math.Acos(dot);

            var limit = Math.PI / 2;
            var dirHeur = (limit - angle) / limit;
            var distHeur = 1 / (dist + 1);
            return (float) (dirHeur * distHeur);
        }

        private static Vector[] RectCorners(Rect rect)
        {
            return new[]
            {
                new Vector(rect.Left, rect.Bottom),
                new Vector(rect.Right, rect.Bottom),
                new Vector(rect.Right, rect.Top),
                new Vector(rect.Left, rect.Top)
            };
        }

        private Rect Closest(Rect rect)
        {
            return rects.OrderBy(
                r => Math.Abs(r.Left - rect.Left) +
                     Math.Abs(r.Right - rect.Right) +
                     Math.Abs(r.Top - rect.Top) +
                     Math.Abs(r.Bottom - rect.Bottom)
            ).First();
        }
    }
}
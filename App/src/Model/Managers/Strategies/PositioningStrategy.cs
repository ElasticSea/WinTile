using System.Collections.Generic;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public abstract class PositioningStrategy
    {
        protected readonly IList<Rect> rects;
        protected readonly IWindowManager windowManager;

        protected PositioningStrategy(IList<Rect> rects, IWindowManager windowManager)
        {
            this.rects = rects;
            this.windowManager = windowManager;
        }
    }
}
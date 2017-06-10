using System;
using System.Collections.Generic;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class PrevNextStrategy : PositioningStrategy
    {
        public PrevNextStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        private Tile NextInDirection(int dir)
        {
            return tiles[(tiles.IndexOf(Selected) + dir + tiles.Count) % tiles.Count];
        }

        public void Prev()
        {
            Selected = NextInDirection(-1);
            windowManager.CurrentWindowRect = Selected.Rect;
        }

        public void Next()
        {
            Selected = NextInDirection(+1);
            windowManager.CurrentWindowRect = Selected.Rect;
        }

        public Tile Selected { get; set; }
        public event Action<Tile> OnSelected;
    }
}
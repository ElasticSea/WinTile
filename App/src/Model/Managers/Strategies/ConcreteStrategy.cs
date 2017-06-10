using System;
using System.Collections.Generic;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class ConcreteStrategy : PositioningStrategy
    {
        public ConcreteStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        public void Position(Tile tile)
        {
            Selected = tile;
            windowManager.CurrentWindowRect = Selected.Rect;
        }

        public Tile Selected { get; set; }
        public event Action<Tile> OnSelected;
    }
}
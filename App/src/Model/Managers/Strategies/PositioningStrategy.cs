using System.Collections.Generic;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public abstract class PositioningStrategy
    {
        protected readonly IList<Tile> tiles;
        protected readonly IWindowManager windowManager;

        protected PositioningStrategy(IList<Tile> tiles, IWindowManager windowManager)
        {
            this.tiles = tiles;
            this.windowManager = windowManager;
        }
    }
}
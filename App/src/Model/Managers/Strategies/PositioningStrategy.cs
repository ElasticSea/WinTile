using System.Collections.Generic;

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
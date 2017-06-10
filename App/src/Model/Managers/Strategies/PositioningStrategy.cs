using System.Collections.Generic;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public abstract class PositioningStrategy
    {
        private readonly SelectedHolder holder;
        protected readonly IList<Tile> tiles;
        protected readonly IWindowManager windowManager;

        protected PositioningStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager)
        {
            this.holder = holder;
            this.tiles = tiles;
            this.windowManager = windowManager;
        }

        public Tile Selected
        {
            get => holder.Selected;
            set => holder.Selected = value;
        }
    }
}
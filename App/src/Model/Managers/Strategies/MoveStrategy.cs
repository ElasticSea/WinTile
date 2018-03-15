using System.Collections.Generic;
using App.Model.Entities;
using App.Model.Managers.Window;

namespace App.Model.Managers.Strategies
{
    public class MoveStrategy : AbstractClosestStrategy
    {
        public MoveStrategy(IList<Tile> tiles, IWindowManager windowManager) : base(tiles, windowManager)
        {
        }

        protected override void OnClosestTIle(Tile tile)
        {
            windowManager.PositionWindow(windowManager.FocusedWindow, tile.Rect);
        }
    }
}
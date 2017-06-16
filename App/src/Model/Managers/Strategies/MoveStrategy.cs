using System.Collections.Generic;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class MoveStrategy : AbstractClosestStrategy
    {
        public MoveStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        protected override void OnClosestTIle(Tile tile)
        {
            windowManager.MoveWindow(windowManager.getCurrentWindow(), tile.Rect);
        }
    }
}
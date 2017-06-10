using System.Collections.Generic;
using System.Linq;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class LayoutStrategy : PositioningStrategy
    {
        public LayoutStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        public void Left()
        {
            var amount = 5;
            var value = Selected.Rect.Left;

            tiles.Where(t => t.Rect.Left == value)
                .ForEach(t => t.Rect.Left -= 5);

            tiles.Where(t => t.Rect.Right == value)
                .ForEach(t => t.Rect.Right -= 5);
            //
            //            var candidates = tiles
            //                .Select(t => t.Rect.Left)
            //                .Where(l => l < Selected.Rect.Left)
            //                .OrderByDescending(t => t);
            //
            //            var left = candidates.Any() ? candidates.First() : (int?) null;
            //
            //            var r = Selected.Rect;
            //            var rect = new Rect(left ?? r.Left, r.Top, r.Right, r.Bottom);
            //            Position(rect);
//            windowManager.CurrentWindowRect = Selected.Rect;
        }

        public void Right()
        {
           
        }

        public void Up()
        {
           
        }

        public void Down()
        {
        
        }
    }
}
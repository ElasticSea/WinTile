using System;
using System.Collections.Generic;

namespace App.Model.Managers
{
    public class TileManager
    {
        public WindowsTileSystem manager;

        private readonly IList<Tile> tiles;
        private readonly IPositioningStrategy closest;
        private readonly IPositioningStrategy extend;
        public Tile Selected { get; set; }
        public event Action<Tile> OnSelected = tile => { };

        public TileManager(IList<Tile> tiles, WindowsTileSystem manager)
        {
            this.tiles = tiles;
            this.manager = manager;
            closest = new ClosestStrategy(tiles);
            extend = new ExtendStrategy(tiles);
        }

        private Tile NextInDirection(int dir)
        {
            return tiles[(tiles.IndexOf(Selected) + dir + tiles.Count) % tiles.Count];
        }

        public void PositionWindow(Tile tile) => manager?.PositionTile(tile);

        public void PositionPrev()
        {
            Selected = NextInDirection(-1);
            PositionWindow(Selected);
        }

        public void PositionNext()
        {
            Selected = NextInDirection(+1);
            PositionWindow(Selected);
        }

        public void PositionClosestRight() => closest.Right(Selected)?.@let(@select);
        public void PositionClosestLeft() => closest.Left(Selected)?.@let(@select);
        public void PositionClosestUp() => closest.Up(Selected)?.@let(@select);
        public void PositionClosestDown() => closest.Down(Selected)?.@let(@select);

        public void PositionExpandRight() => extend.Right(Selected)?.@let(@select);
        public void PositionExpandLeft() => extend.Left(Selected)?.@let(@select);
        public void PositionExpandUp() => extend.Up(Selected)?.@let(@select);
        public void PositionExpandDown() => extend.Down(Selected)?.@let(@select);


        private void @select(Tile s)
        {
            OnSelected(s);
            Selected = s;
            PositionWindow(Selected);
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.Model.Managers
{
    public class TileManager
    {
        public Tile Selected;

        private ObservableCollection<Tile> tiles;

        public TileManager(ObservableCollection<Tile> tiles)
        {
            this.tiles = tiles;
        }

        public void Add(Tile tile)
        {
            tiles.Add(tile);
            Selected = tile;
        }

        public void Add(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                Add(tile);
            }
        }

        public void Set(IEnumerable<Tile> tiles)
        {
            Clear();
            Add(tiles);
        }

        public void Remove(Tile tile)
        {
            tiles.Remove(tile);
            Selected = tiles.FirstOrDefault(t => t == Selected);
        }

        public Tile MovePrev()
        {
            Selected = NextInDirection(-1);
            return Selected;
        }

        public Tile MoveNext()
        {
            Selected = NextInDirection(+1);
            return Selected;
        }

        private Tile NextInDirection(int dir) => tiles[(tiles.IndexOf(Selected) + dir + tiles.Count) % tiles.Count];

        public void Clear()
        {
            Selected = null;
            tiles.Clear();
        }
    }
}
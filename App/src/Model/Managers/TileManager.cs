using System.Collections.ObjectModel;
using System.Linq;

namespace App.Model.Managers
{
    public class TileManager
    {
        public Tile Selected;

        public ObservableCollection<Tile> Tiles { get; } = new ObservableCollection<Tile>();

        public void Add(Tile tile)
        {
            Tiles.Add(tile);
            Selected = tile;
        }

        public void Remove(Tile tile)
        {
            Tiles.Remove(tile);
            Selected = Tiles.FirstOrDefault(t => t == Selected);
        }

        public void MovePrev() => Selected = NextInDirection(-1);
        public void MoveNext() => Selected = NextInDirection(+1);
        private Tile NextInDirection(int dir) => Tiles[(Tiles.IndexOf(Selected) + dir + Tiles.Count) % Tiles.Count];

        public void Clear()
        {
            Selected = null;
            Tiles.Clear();
        }
    }
}
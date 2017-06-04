using System.Collections.ObjectModel;

namespace App.Model
{
    public class Layout
    {
        public Hotkey PreviousTile;
        public Hotkey NextTile;
        public ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();
    }
}
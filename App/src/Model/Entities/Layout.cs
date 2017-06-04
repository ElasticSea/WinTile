using System.Collections.ObjectModel;

namespace App.Model
{
    public class Layout
    {
        public Hotkey PreviousTile;
        public Hotkey NextTile;
        public Hotkey ClosestLeft;
        public Hotkey ClosestRight;
        public Hotkey ClosestUp;
        public Hotkey ClosestDown;
        public ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();
    }
}
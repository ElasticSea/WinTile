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
        public Hotkey ExpandLeft;
        public Hotkey ExpandRight;
        public Hotkey ExpandUp;
        public Hotkey ExpandDown;
        public ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();
    }
}
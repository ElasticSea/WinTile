namespace App.Model
{
    public class Layout
    {
        public Hotkey PreviousTile;
        public Hotkey NextTile;
        public TrulyObservableCollection<Tile> tiles = new TrulyObservableCollection<Tile>();
    }
}
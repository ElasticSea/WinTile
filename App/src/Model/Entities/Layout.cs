using System.Collections.ObjectModel;

namespace App.Model
{
    public class Layout
    {
        public ObservableCollection<HotkeyPair> hotkeys = new ObservableCollection<HotkeyPair>();
        public ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();
    }
}
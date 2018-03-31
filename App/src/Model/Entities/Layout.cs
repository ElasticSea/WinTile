using System.Collections.ObjectModel;

namespace ElasticSea.Wintile.Model.Entities
{
    public class Layout
    {
        public ObservableCollection<HotkeyPair> hotkeys = new ObservableCollection<HotkeyPair>();
        public Grid Grid { get; set; } = new Grid();
    }
}
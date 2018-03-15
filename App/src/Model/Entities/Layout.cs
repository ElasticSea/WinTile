using System.Collections.ObjectModel;

namespace App.Model.Entities
{
    public class Layout
    {
        public ObservableCollection<HotkeyPair> hotkeys = new ObservableCollection<HotkeyPair>();
        public Grid Grid  { get; set; } = new Grid();
    }
}
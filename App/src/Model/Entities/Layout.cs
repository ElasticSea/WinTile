using System.Collections.ObjectModel;
using App.Model.Entities;

namespace App.Model
{
    public class Layout
    {
        public ObservableCollection<HotkeyPair> hotkeys = new ObservableCollection<HotkeyPair>();
        public Grid Grid  { get; set; } = new Grid();
    }
}
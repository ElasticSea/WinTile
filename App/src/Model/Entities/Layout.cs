using System.Collections.ObjectModel;

namespace App.Model
{
    public class Layout
    {
        public ObservableCollection<HotkeyPair> hotkeys = new ObservableCollection<HotkeyPair>();
        public ObservableCollection<Handle> VerticalHandlers { get; set; } = new ObservableCollection<Handle>();
        public ObservableCollection<Handle> HorizontalHandlers { get; set; } = new ObservableCollection<Handle>();
    }
}
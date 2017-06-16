using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class HotkeyPair
    {
        public HotkeyType Type { get; set; }
        public Hotkey Hotkey { get; set; }

        public HotkeyPair(HotkeyType type = HotkeyType.None, Hotkey hotkey = null)
        {
            Type = type;
            Hotkey = hotkey;
        }
    }
}
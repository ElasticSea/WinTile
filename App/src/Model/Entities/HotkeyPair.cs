using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class HotkeyPair
    {
        public HotkeyPair(HotkeyType type = HotkeyType.ExpandDown, Hotkey hotkey = null)
        {
            Type = type;
            Hotkey = hotkey;
        }

        public HotkeyType Type { get; set; }
        public Hotkey Hotkey { get; set; }
    }
}
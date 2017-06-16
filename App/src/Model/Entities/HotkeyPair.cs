namespace App.Model
{
    public class HotkeyPair
    {
        public HotkeyType Type { get; set; }
        public Hotkey Hotkey { get; set; }

        public HotkeyPair(HotkeyType type = HotkeyType.None, Hotkey hotkey = null)
        {
            Type = type;
            Hotkey = hotkey;
        }

        protected bool Equals(HotkeyPair other)
        {
            return Type == other.Type && Equals(Hotkey, other.Hotkey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HotkeyPair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type * 397) ^ (Hotkey != null ? Hotkey.GetHashCode() : 0);
            }
        }
    }
}
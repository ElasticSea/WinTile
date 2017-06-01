using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Input;

namespace WinTile.Model
{
    public class WindowTile
    {
        public Rect tile;

        [JsonConverter(typeof(StringEnumConverter))]
        public Key hotkey;

        public WindowTile(Rect tile, Key hotkey = Key.None)
        {
            this.tile = tile;
            this.hotkey = hotkey;
        }
    }
}
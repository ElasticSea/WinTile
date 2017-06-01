using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinTile.Model
{
    public class Hotkey
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Key key;

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<KeyModifier> modifiers;

        public Hotkey(Key key, List<KeyModifier> modifiers)
        {
            this.key = key;
            this.modifiers = modifiers;
        }
    }
}
using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace App.Model
{
    public class Hotkey
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Key key;

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<KeyModifier> modifiers;

        public Hotkey(Key key = Key.None, List<KeyModifier> modifiers = null)
        {
            this.key = key;
            this.modifiers = modifiers;
        }
    }
}
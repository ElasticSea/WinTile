using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace App.Model
{
    public class Hotkey
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Key key;

        [JsonConverter(typeof(StringEnumConverter))]
        public KeyModifier modifiers;

        public Hotkey(Key key = Key.None, KeyModifier modifiers = KeyModifier.None)
        {
            this.key = key;
            this.modifiers = modifiers;
        }

        public override string ToString()
        {
            return $"{key}, {string.Join(", ",modifiers)}";
        }
    }
}
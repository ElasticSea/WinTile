using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace App.Model
{
    public class Hotkey
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Key Key { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public KeyModifier Modifiers { get; }

        public Hotkey(Key key = Key.None, KeyModifier modifiers = KeyModifier.None)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public override string ToString()
        {
            return $"{Key}, {string.Join(", ",Modifiers)}";
        }
    }
}
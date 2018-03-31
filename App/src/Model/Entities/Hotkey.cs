using System.Windows.Input;
using ElasticSea.Wintile.Model.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElasticSea.Wintile.Model.Entities
{
    public class Hotkey
    {
        public Hotkey(Key key = Key.None, KeyModifier modifiers = KeyModifier.None)
        {
            Key = key;
            Modifiers = modifiers;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Key Key { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public KeyModifier Modifiers { get; }

        public override string ToString()
        {
            return $"{Key}, {string.Join(", ", Modifiers)}";
        }

        protected bool Equals(Hotkey other)
        {
            return Key == other.Key && Modifiers == other.Modifiers;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Hotkey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key * 397) ^ (int) Modifiers;
            }
        }

        public static bool operator ==(Hotkey left, Hotkey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Hotkey left, Hotkey right)
        {
            return !Equals(left, right);
        }
    }
}
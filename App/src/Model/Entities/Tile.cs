using System.Windows.Media;
using Newtonsoft.Json;
using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Tile
    {
        public Tile(Rect rect)
        {
            Rect = rect;
        }

        [JsonIgnore]
        public bool Selected { get; set; }

        [JsonIgnore]
        public SolidColorBrush Bursh { get; set; }

        public Rect Rect { get; set; }

        public override string ToString()
        {
            return $"{nameof(Rect)}: {Rect}";
        }
    }
}
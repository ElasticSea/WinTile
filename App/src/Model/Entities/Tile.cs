using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Annotations;

namespace App.Model
{
    public class Tile : INotifyPropertyChanged
    {
        public Rect Rect { get; }
        public Hotkey Hotkey { get; set; }

        public Tile(Rect rect, Hotkey hotkey = null)
        {
            Rect = rect;
            Hotkey = hotkey;
        }

        public override string ToString()
        {
            return $"{nameof(Rect)}: {Rect}, {nameof(Hotkey)}: {Hotkey}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using WinTile.Model;

namespace WinTile
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event Action<WindowTile> WindowAdded = rect => { };
        public event Action<WindowTile> WindowRemoved = rect => { };
        public event Action<WindowTile> WindowChanged = rect => { };

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        private readonly Layout layout = JsonConvert.DeserializeObject<Layout>(Properties.Settings.Default.Layout ?? "{}") ?? new Layout();
        private WindowTile selected;
        private float _left = 0f;
        private float _top = 0f;
        private float _right = 0f;
        private float _bottom = 0f;

        internal void Load()
        {
            foreach (var windowTile in layout.windows)
            {
                WindowAdded(windowTile);
            }
        }

        public float Left
        {
            get { return _left; }
            set
            {
                if(value == _left) return;

                _left = Math.Max(value, 0f);
                Right = Math.Max(_left, _right);
                TriggerTileChanged();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Left)));
            }
        }

        private void TriggerTileChanged()
        {
            if (selected != null)
            {
                selected.tile.Left  = Left / 100;
                selected.tile.Right = Right / 100;
                selected.tile.Top = Top / 100;
                selected.tile.Bottom = Bottom / 100;

                WindowChanged(selected);
            }
        }

        public float Top
        {
            get { return _top; }
            set
            {
                if (value == _top) return;

                _top = Math.Max(value, 0f);
                Bottom = Math.Max(_top, _bottom);
                TriggerTileChanged();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Top)));
            }
        }

        public float Right
        {
            get { return _right; }
            set
            {
                if (value == _right) return;

                _right = Math.Min(value, 100f);
                Left = Math.Min(_left, _right);
                TriggerTileChanged();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Right)));
            }
        }

        public float Bottom
        {
            get { return _bottom; }
            set
            {
                if (value == _bottom) return;

                _bottom = Math.Min(value, 100f);
                Top = Math.Min(_top, _bottom);
                TriggerTileChanged();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bottom)));
            }
        }

        internal WindowTile Selected
        {
            get { return selected; }
            set
            {
                selected = value;

                if (selected != null)
                {
                    _left = selected.tile.Left * 100;
                    _right = selected.tile.Right * 100;
                    _top = selected.tile.Top * 100;
                    _bottom = selected.tile.Bottom * 100;

                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Left)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Top)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Right)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bottom)));
                }
            }
        }

        public void AddWindow()
        {
            var window = new WindowTile(new Rect(Left, Top, Right, Bottom) / 100f);
            layout.windows.Add(window);
            WindowAdded(window);
        }

        public void removeWindow(WindowTile window)
        {
            layout.windows.Remove(window);
            WindowRemoved(window);
        }

        internal void Save()
        {
            Properties.Settings.Default.Layout = JsonConvert.SerializeObject(layout);
            Properties.Settings.Default.Save();
        }
    }
}
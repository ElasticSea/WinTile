using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json;
using WinTile.Model;
using WinTile.Properties;
using static System.Windows.SystemParameters;

namespace WinTile
{
    public class ViewModel : INotifyPropertyChanged
    {
        private float _bottom;
        private float _left;
        private float _right;
        private float _top;

        private readonly List<HotKey> hotkeys = new List<HotKey>();

        private Layout layout;

        private WindowTile selected;

        public Layout Layout
        {
            get => layout;
            set
            {
                layout = value;
                OnNewLayout();
            }
        }

        public string JsonLayout
        {
            get => JsonConvert.SerializeObject(Layout, Formatting.Indented);
            set => Layout = JsonConvert.DeserializeObject<Layout>(value) ?? new Layout();
        }

        public float Left
        {
            get => _left;
            set
            {
                if (value == _left) return;

                _left = Math.Max(value, 0f);
                Right = Math.Max(_left, _right);
                TriggerTileChanged();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Left)));
            }
        }

        public float Top
        {
            get => _top;
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
            get => _right;
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
            get => _bottom;
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
            get => selected;
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

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };
        public event Action<WindowTile> WindowAdded = rect => { };
        public event Action<WindowTile> WindowRemoved = rect => { };
        public event Action<WindowTile> WindowChanged = rect => { };

        public void Load()
        {
            JsonLayout = Settings.Default.Layout ?? "{}";
        }

        private void OnNewLayout()
        {
            hotkeys.ForEach(h => h.Unregister());
            foreach (var windowTile in Layout.windows)
            {
                hotkeys.Add(new HotKey(windowTile.hotkey, KeyModifier.Shift | KeyModifier.Win,
                    key => PositionWindow(windowTile)));
                WindowAdded(windowTile);
            }
        }

        private void PositionWindow(WindowTile windowTile)
        {
            var pxRect = windowTile.tile.extend(WorkArea.Width, WorkArea.Height);
            Console.WriteLine($"Moving window to [{pxRect}]");
            User32Utils.SetCurrentWindowPos((int) pxRect.Left, (int) pxRect.Top, (int) pxRect.Width,
                (int) pxRect.Height);
        }

        private void TriggerTileChanged(Key hotkey = Key.None)
        {
            if (selected != null)
            {
                selected.tile.Left = Left / 100;
                selected.tile.Right = Right / 100;
                selected.tile.Top = Top / 100;
                selected.tile.Bottom = Bottom / 100;

                if (hotkey != Key.None) selected.hotkey = hotkey;

                WindowChanged(selected);
            }
        }

        public void updateHotKey(KeyEventArgs args)
        {
            TriggerTileChanged(args.Key);
        }

        public void AddWindow()
        {
            var window = new WindowTile(new Rect(Left, Top, Right, Bottom) / 100f);
            Layout.windows.Add(window);
            WindowAdded(window);
        }

        public void removeWindow(WindowTile window)
        {
            Layout.windows.Remove(window);
            WindowRemoved(window);
        }

        internal void Save()
        {
            Settings.Default.Layout = JsonLayout;
            Settings.Default.Save();
        }
    }
}
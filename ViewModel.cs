using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json;
using PropertyChanged;
using WinTile.Model;
using WinTile.Properties;
using static System.Windows.SystemParameters;

namespace WinTile
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
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

        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        internal WindowTile Selected
        {
            get => selected;
            set
            {
                selected = value;

                if (selected != null)
                {
                    Left = selected.tile.Left * 100;
                    Right = selected.tile.Right * 100;
                    Top = selected.tile.Top * 100;
                    Bottom = selected.tile.Bottom * 100;
                }
            }
        }

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

        public void TriggerTileChanged(Key hotkey = Key.None)
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
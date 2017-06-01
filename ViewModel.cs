using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using PropertyChanged;
using WinTile.Model;
using WinTile.Properties;
using static System.Windows.SystemParameters;
using Rect = WinTile.Model.Rect;

namespace WinTile
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        private readonly List<HotKeyUtils> hotkeys = new List<HotKeyUtils>();

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
                    Left = selected.rect.Left * 100;
                    Right = selected.rect.Right * 100;
                    Top = selected.rect.Top * 100;
                    Bottom = selected.rect.Bottom * 100;
                }
            }
        }

        public event Action<WindowTile> WindowAdded = rect => { };
        public event Action<WindowTile> WindowRemoved = rect => { };
        public event Action<WindowTile> WindowChanged = rect => { };

        public void Load()
        {
            try
            {
            JsonLayout = Settings.Default.Layout ?? "{}";
            }
            catch (JsonSerializationException e)
            {
                JsonLayout = "{}";
                MessageBox.Show("User Profile is corrupted: " + e.Message, "Corrupted Data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnNewLayout()
        {
            hotkeys.ForEach(h => h.Unregister());
            foreach (var windowTile in Layout.windows)
            {
                if (windowTile.hotkey != null)
                {
                    var key = windowTile.hotkey.key;
                    var keyModifiers = windowTile.hotkey.modifiers.Aggregate((a, b) => a | b);
                    hotkeys.Add(new HotKeyUtils(key, keyModifiers, k => PositionWindow(windowTile)));
                }
                WindowAdded(windowTile);
            }
        }

        private void PositionWindow(WindowTile windowTile)
        {
            var pxRect = windowTile.rect.extend(WorkArea.Width, WorkArea.Height);
            Console.WriteLine($"Moving window to [{pxRect}]");
            User32Utils.SetCurrentWindowPos((int) pxRect.Left, (int) pxRect.Top, (int) pxRect.Width,
                (int) pxRect.Height);
        }

        public void TriggerTileChanged()
        {
            if (selected != null)
            {
                selected.rect.Left = Left / 100;
                selected.rect.Right = Right / 100;
                selected.rect.Top = Top / 100;
                selected.rect.Bottom = Bottom / 100;

                WindowChanged(selected);
            }
        }

        public void TriggerHotkeyChanged(Key key, List<KeyModifier> modifiers)
        {
            if (selected != null && key != Key.None)
            {
                selected.hotkey = new Hotkey(key, modifiers);
                WindowChanged(selected);
            }
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
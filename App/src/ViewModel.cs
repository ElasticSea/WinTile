using System;
using System.Collections.ObjectModel;
using System.Windows;
using App.Model;
using App.Model.Managers;
using App.Properties;
using Newtonsoft.Json;
using PropertyChanged;
using Rect = App.Model.Rect;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        private readonly TileManager tileManager;
        private readonly HotkeyManager hotkeyManager;

        public ViewModel()
        {
            Reload();

            tileManager = new TileManager(Layout.tiles);
            hotkeyManager = new HotkeyManager(Layout, tileManager);
        }

        private Layout Layout { get; set; }
        public ObservableCollection<Tile> Tiles => Layout.tiles;

        public Hotkey PrevTile
        {
            get => Layout.PreviousTile;
            set => Layout.PreviousTile = value;
        }

        public Hotkey NextTile
        {
            get => Layout.NextTile;
            set => Layout.NextTile = value;
        }

        public Tile Selected
        {
            get => tileManager.Selected;
            set => tileManager.Selected = value;
        }

        public Hotkey SelectedHotkey
        {
            get => Selected?.Hotkey;
            set => Selected?.let(s => s.Hotkey = value);
        }

        public string JsonLayout
        {
            get => JsonConvert.SerializeObject(Layout, Formatting.Indented);
            set
            {
                try
                {
                    Layout = JsonConvert.DeserializeObject<Layout>(value) ?? new Layout();
                }
                catch (Exception e)
                {
                    JsonLayout = "{}";
                    MessageBox.Show("User Profile is corrupted: " + e.Message, "Corrupted Data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }

        public void Reload()
        {
            JsonLayout = Settings.Default.Layout ?? "{}";
        }

        public void BindHotkeys() => hotkeyManager.BindHotkeys();

        public void UnbindHotkeys() => hotkeyManager.UnbindHotkeys();

        public void AddTile()
        {
            var r = Selected?.Rect ?? new Rect();
            var rect = new Rect(r.Left, r.Top, r.Right, r.Bottom);
            Tiles.Add(new Tile(rect));
        }

        public void RemoveTile(Tile window)
        {
            Tiles.Remove(window);
        }

        internal void Save()
        {
            Settings.Default.Layout = JsonLayout;
            Settings.Default.Save();
        }

        public void MoveSelectedUp()
        {
            var index = Tiles.IndexOf(Selected);
            if (index > 0)
                Tiles.Move(index, index - 1);
        }

        public void MoveSelectedDown()
        {
            var index = Tiles.IndexOf(Selected);
            if (index < Tiles.Count - 1)
                Tiles.Move(index, index + 1);
        }
    }
}
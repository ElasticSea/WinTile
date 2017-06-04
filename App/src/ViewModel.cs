using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using App.Model;
using App.Model.Managers;
using App.Properties;
using Newtonsoft.Json;
using PropertyChanged;
using Rect = App.Model.Rect;
using static System.Windows.SystemParameters;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        public HotkeyManager hotkeyManager { get; }

        private readonly TileManager tileManager;

        public ViewModel()
        {
            Reload();

            hotkeyManager = new HotkeyManager();
            tileManager = new TileManager(Layout.tiles);

            PrevTile = Layout.PreviousTile;
            NextTile = Layout.NextTile;
        }

        public ObservableCollection<Tile> Tiles => Layout.tiles;
        public Hotkey PrevTile
        {
            get => Layout.PreviousTile;
            set
            {
                if (value == null)
                {
                    hotkeyManager.Unregister(PrevTile);
                    Layout.PreviousTile = null;
                }
                else if (hotkeyManager.Register(value, h => tileManager.MovePrev().let(PositionWindow)))
                {
                    hotkeyManager.Unregister(PrevTile);
                    Layout.PreviousTile = value;
                }
                // TODO raise error
            }
        }

        public Hotkey NextTile
        {
            get => Layout.NextTile;
            set
            {
                if (value == null)
                {
                    hotkeyManager.Unregister(NextTile);
                    Layout.NextTile = null;
                }
                else if (hotkeyManager.Register(value, h => tileManager.MoveNext().let(PositionWindow)))
                {
                    hotkeyManager.Unregister(NextTile);
                    Layout.NextTile = value;
                }
                // TODO raise error
            }
        }

        public Tile Selected
        {
            get => tileManager.Selected;
            set => tileManager.Selected = value;
        }

        public int Left
        {
            get => Selected?.Rect.Left ?? 0;
            set
            {
                Selected?.@let(s => s.Rect.Left = value);
//                Tiles.CollectionChanged()
//                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
//                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tiles)));
            }
        }

        private void PositionWindow(Tile tile)
        {
            var pxRect = tile.Rect.extend((int) WorkArea.Width, (int) WorkArea.Height) / 100;
            User32Utils.SetCurrentWindowPos(pxRect.Left, pxRect.Top, pxRect.Width, pxRect.Height);
        }

        public Hotkey SelectedHotkey
        {
            get => Selected?.Hotkey;
            set => Selected?.@let(s => s.Hotkey = value);
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

        private Layout Layout { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public void Reload()
        {
                JsonLayout = Settings.Default.Layout ?? "{}";
        }

//        public void TriggerTileChanged()
//        {
//            if (selected != null)
//            {
//                selected.rect.Left = Left / 100;
//                selected.rect.Right = Right / 100;
//                selected.rect.Top = Top / 100;
//                selected.rect.Bottom = Bottom / 100;
//
//                WindowChanged(selected);
//            }
//        }

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
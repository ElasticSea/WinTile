using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using App.Model;
using App.Model.Managers;
using App.Properties;
using Newtonsoft.Json;
using PropertyChanged;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly List<HotKeyUtils> hotkeys = new List<HotKeyUtils>();
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => {};

        //
        //        public float Left { get; set; }
        //        public float Top { get; set; }
        //        public float Right { get; set; }
        //        public float Bottom { get; set; }

        //        internal Tile Selected
        //        {
        //            get => selected;
        //            set
        //            {
        //                selected = value;
        //
        //                if (selected != null)
        //                {
        //                    Left = selected.rect.Left * 100;
        //                    Right = selected.rect.Right * 100;
        //                    Top = selected.rect.Top * 100;
        //                    Bottom = selected.rect.Bottom * 100;
        //                }
        //            }
        //        }

        //        public event Action<Tile> WindowAdded = rect => { };
        //        public event Action<Tile> WindowRemoved = rect => { };
        //        public event Action<Tile> WindowChanged = rect => { };


        private Layout layout;

        private Tile selected;
        private TileManager tileManager;
        private PositionWIndowManager positionManager;

        public ViewModel()
        {
            tileManager = new TileManager();
            positionManager = new PositionWIndowManager(tileManager);

            try
            {
                JsonLayout = Settings.Default.Layout ?? "{}";
            }
            catch (JsonSerializationException e)
            {
                JsonLayout = "{}";
                MessageBox.Show("User Profile is corrupted: " + e.Message, "Corrupted Data", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        public ObservableCollection<Tile> Tiles => tileManager.Tiles;

        public Tile Selected
        {
            get => tileManager.Selected;
            set => tileManager.Selected = value;
        }

        public string JsonLayout
        {
            get => JsonConvert.SerializeObject(Layout, Formatting.Indented);
            set => Layout = JsonConvert.DeserializeObject<Layout>(value) ?? new Layout();
        }

        public Layout Layout
        {
            get => layout;
            set
            {
                layout = value;
                tileManager.Clear();
                foreach (var tile in layout.windows)
                {
                    tileManager.Add(tile);
                }
            }
        }

//        private void PositionWindow(Tile tile)
//        {
//        }
//
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
//
//        public void TriggerHotkeyChanged(Key key, List<KeyModifier> modifiers)
//        {
//            if (selected != null && key != Key.None)
//            {
//                selected.hotkey = new Hotkey(key, modifiers);
//                WindowChanged(selected);
//            }
//        }

        public void AddWindow()
        {
//            var window = new Tile(new Model.Rect(Left, Top, Right, Bottom) / 100f);
//            Layout.windows.Add(window);
//            WindowAdded(window);
        }

        public void removeWindow(Tile window)
        {
//            Layout.windows.Remove(window);
//            WindowRemoved(window);
        }

        internal void Save()
        {
            Settings.Default.Layout = JsonLayout;
            Settings.Default.Save();
        }

        public void NextTile()
        {
            tileManager.MoveNext();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
        }

        public void PrevTile()
        {
            tileManager.MovePrev();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
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
            if(index < Tiles.Count - 1)
            Tiles.Move(index, index + 1);
        }
    }
}
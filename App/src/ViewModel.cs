using System.Collections.ObjectModel;
using System.ComponentModel;
using App.Model;
using App.Model.Managers;
using App.Properties;
using PropertyChanged;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => {};

        private readonly LayoutManager layoutManager = new LayoutManager();
        private readonly WindowsTileSystem windowsTileManager = new WindowsTileSystem();
        private bool _activeInEditor;
        private HotkeyManager hotkeyManager;
        private TileManager tileManager;

        public ViewModel()
        {
            Reload();
        }

        public ObservableCollection<Tile> Tiles => layoutManager.Layout.tiles;

        public Hotkey PrevTile
        {
            get => layoutManager.Layout.PreviousTile;
            set => layoutManager.Layout.PreviousTile = value;
        }

        public string JsonLayout
        {
            get => layoutManager.Json;
            set
            {
                layoutManager.Json = value;
                tileManager = new TileManager(layoutManager.Layout.tiles, windowsTileManager);
                tileManager.OnSelected += tile => { Selected = tile; };
                hotkeyManager = new HotkeyManager(layoutManager.Layout, tileManager);
            }
        }

        public Hotkey NextTile
        {
            get => layoutManager.Layout.NextTile;
            set => layoutManager.Layout.NextTile = value;
        }


        public Hotkey ClosestLeft
        {
            get => layoutManager.Layout.ClosestLeft;
            set => layoutManager.Layout.ClosestLeft = value;
        }


        public Hotkey ClosestRight
        {
            get => layoutManager.Layout.ClosestRight;
            set => layoutManager.Layout.ClosestRight = value;
        }


        public Hotkey ClosestUp
        {
            get => layoutManager.Layout.ClosestUp;
            set => layoutManager.Layout.ClosestUp = value;
        }

        public Hotkey ClosestDown
        {
            get => layoutManager.Layout.ClosestDown;
            set => layoutManager.Layout.ClosestDown = value;
        }


        public Hotkey ExpandLeft
        {
            get => layoutManager.Layout.ExpandLeft;
            set => layoutManager.Layout.ExpandLeft = value;
        }


        public Hotkey ExpandRight
        {
            get => layoutManager.Layout.ExpandRight;
            set => layoutManager.Layout.ExpandRight = value;
        }


        public Hotkey ExpandUp
        {
            get => layoutManager.Layout.ExpandUp;
            set => layoutManager.Layout.ExpandUp = value;
        }

        public Hotkey ExpandDown
        {
            get => layoutManager.Layout.ExpandDown;
            set => layoutManager.Layout.ExpandDown = value;
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

        public bool ActiveInEditor
        {
            get => _activeInEditor;
            set
            {
                _activeInEditor = value;

                if (ActiveInEditor)
                    hotkeyManager.BindHotkeys();
                else
                    hotkeyManager.UnbindHotkeys();
            }
        }

        public void Reload()
        {
            JsonLayout = Settings.Default.Layout ?? "{}";
        }

        public void BindHotkeys()
        {
            tileManager.manager = windowsTileManager;
            hotkeyManager.BindHotkeys();
        }

        public void UnbindHotkeys()
        {
            tileManager.manager = null;
            hotkeyManager.UnbindHotkeys();
        }

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
            Settings.Default.Layout = layoutManager.Json;
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
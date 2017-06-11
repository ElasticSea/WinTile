using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using App.Model;
using App.Model.Managers;
using App.Model.Managers.Strategies;
using App.Properties;
using App.Utils;
using PropertyChanged;
using Rect = App.Model.Rect;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => {};

        private readonly LayoutManager layoutManager = new LayoutManager();
        private bool _activeInEditor;
        private HotkeyManager hotkeyManager;

        private readonly SelectedHolder holder = new SelectedHolder();
        private readonly CompositeWindowManager windowManager = new CompositeWindowManager(new ConvertWindowManager(new User32Manager()),new WindowManagerDummy());
        private ClosestStrategy b;
        private ExtendStrategy c;
        private LayoutStrategy d;

        public ObservableCollection<Tile> Tiles
        {
            get => layoutManager.Layout.tiles;
            set => layoutManager.Layout.tiles = value;
        }

        public string JsonLayout
        {
            get => layoutManager.Json;
            set
            {
                layoutManager.Json = value;
                
                b = new ClosestStrategy(holder, layoutManager.Layout.tiles, windowManager);
                c = new ExtendStrategy(holder, layoutManager.Layout.tiles, windowManager);
                d = new LayoutStrategy(holder, layoutManager.Layout.tiles, windowManager);

                holder.OnSelected += tile =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tiles)));
                };

                hotkeyManager = new HotkeyManager(layoutManager.Layout, b,c,d);
            }
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


        public Hotkey LayoutLeft
        {
            get => layoutManager.Layout.LayoutLeft;
            set => layoutManager.Layout.LayoutLeft = value;
        }


        public Hotkey LayoutRight
        {
            get => layoutManager.Layout.LayoutRight;
            set => layoutManager.Layout.LayoutRight = value;
        }


        public Hotkey LayoutUp
        {
            get => layoutManager.Layout.LayoutUp;
            set => layoutManager.Layout.LayoutUp = value;
        }

        public Hotkey LayoutDown
        {
            get => layoutManager.Layout.LayoutDown;
            set => layoutManager.Layout.LayoutDown = value;
        }

        public Tile Selected
        {
            get => holder.Selected;
            set => holder.Selected = value;
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

        public void Load()
        {
            JsonLayout = Settings.Default.Layout ?? "{}";
        }

        public void BindHotkeys()
        {
            windowManager.Active = true;
            hotkeyManager.BindHotkeys();
        }

        public void UnbindHotkeys()
        {
            windowManager.Active = false;
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
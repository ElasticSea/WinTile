﻿using System;
using System.Collections.Generic;
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

        public ObservableCollection<HotkeyPair> Hotkeys
        {
            get => layoutManager.Layout.hotkeys;
            set => layoutManager.Layout.hotkeys = value;
        }

        public IEnumerable<HotkeyType> HotkeyTypes
        {
            get
            {
                var used = Hotkeys.Select(h => h.Type);
                var types = Enum.GetValues(typeof(HotkeyType)).Cast<HotkeyType>();
                return types.Except(used);
            }
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

        public Tile Selected
        {
            get => holder.Selected;
            set => holder.Selected = value;
        }

        public HotkeyPair SelectedHotkeyPair { get; set; }
        
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

        public void AddHotkey()
        {
            var p = SelectedHotkeyPair ?? new HotkeyPair();
            var pair = new HotkeyPair(p.Type, p.Hotkey);
            Hotkeys.Add(pair);
        }

        public void RemoveHotkey()
        {
            throw new NotImplementedException();
        }
    }
}
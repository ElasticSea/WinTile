using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using App.Model;
using App.Model.Entities;
using App.Model.Managers;
using App.Model.Managers.Strategies;
using App.Properties;
using App.Utils;
using PropertyChanged;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly LayoutManager layoutManager = new LayoutManager();
        private bool _activeHotkeys;
        private Tile _selected;
        private HotkeyPair _selectedHotkeyPair;
        private SandboxWindowManager sandbox;
        private HotkeyManager hotkeyManager;
        private ConvertWindowManager nativeWindowManager;
        private readonly CompositeWindowManager windowManager = new CompositeWindowManager();
        private CuttingManager cuttingManager;
        private bool _enterSandboxMode;

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
                reload();
            }
        }

        private void reload()
        {
            cuttingManager = new CuttingManager(layoutManager.Layout.Grid);
            nativeWindowManager = new ConvertWindowManager(new User32Manager());
            sandbox = new SandboxWindowManager(Tiles);
            windowManager.CurrentManager = sandbox;

            var move = new MoveStrategy(Tiles, windowManager);
            var select = new SelectStrategy(Tiles, windowManager);
            var extend = new ExtendStrategy(Tiles, windowManager);
//                var layout = new LayoutStrategy(Tiles, windowManager);

            hotkeyManager = new HotkeyManager(layoutManager.Layout.hotkeys,
                new Dictionary<HotkeyType, Action<object>>
                {
                    {HotkeyType.MoveLeft, h1 => move.Left()},
                    {HotkeyType.MoveRight, h1 => move.Right()},
                    {HotkeyType.MoveUp, h1 => move.Up()},
                    {HotkeyType.MoveDown, h1 => move.Down()},

                    {HotkeyType.ExpandLeft, h1 => extend.Left()},
                    {HotkeyType.ExpandRight, h1 => extend.Right()},
                    {HotkeyType.ExpandUp, h1 => extend.Up()},
                    {HotkeyType.ExpandDown, h1 => extend.Down()},
//
//                        {HotkeyType.LayoutLeft, h1 => layout.Left()},
//                        {HotkeyType.LayoutRight, h1 => layout.Right()},
//                        {HotkeyType.LayoutUp, h1 => layout.Up()},
//                        {HotkeyType.LayoutDown, h1 => layout.Down()},

                    {HotkeyType.SelectLeft, h1 => @select.Left()},
                    {HotkeyType.SelectRight, h1 => @select.Right()},
                    {HotkeyType.SelectUp, h1 => @select.Up()},
                    {HotkeyType.SelectDown, h1 => @select.Down()}
                });
        }

        public ObservableCollection<Handle> Rows => layoutManager.Layout.Grid.Rows;
        public ObservableCollection<Handle> Columns => layoutManager.Layout.Grid.Columns;
        public ObservableCollection<Tile> Tiles => cuttingManager.Tiles;
        public ObservableCollection<HotkeyPair> Hotkeys => layoutManager.Layout.hotkeys;
        public ObservableCollection<Window> Windows => sandbox.Windows;

        public HotkeyPair SelectedHotkeyPair { private get; set; }
        public HotkeyType AddHotkeyType { get; set; }
        public Hotkey AddHotkeyHotkey { get; set; }

        public bool ActiveHotkeys
        {
            get => _activeHotkeys;
            set
            {
                _activeHotkeys = value;

                if (ActiveHotkeys)
                    hotkeyManager.BindHotkeys();
                else
                    hotkeyManager.UnbindHotkeys();
            }
        }

        public bool EnterSandboxMode
        {
            get => _enterSandboxMode;
            set
            {
                _enterSandboxMode = value;

                if (EnterSandboxMode)
                {
                    ActiveHotkeys = true;
                    windowManager.CurrentManager = sandbox;
                }
                else
                {
                    windowManager.CurrentManager = nativeWindowManager;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public void Load()
        {
            JsonLayout = Settings.Default.Layout ?? "{}";
        }

        public void AddWindow()
        {
            sandbox.AddWindow();
        }

        public void RemoveWindow()
        {
            sandbox.RemoveWindow();
        }

        internal void Save()
        {
            Settings.Default.Layout = layoutManager.Json;
            Settings.Default.Save();
        }

        public void AddHotkey()
        {
            var htKey = new Hotkey(AddHotkeyHotkey.Key, AddHotkeyHotkey.Modifiers);
            var pair = new HotkeyPair(AddHotkeyType, htKey);
            Hotkeys.Add(pair);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HotkeyTypes)));
        }

        public void RemoveHotkey()
        {
            Hotkeys.Remove(SelectedHotkeyPair);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HotkeyTypes)));
        }

        public void CutVertical() => cuttingManager.CutVertical();
        public void CutHorizontal() => cuttingManager.CutHorizontal();
    }
}
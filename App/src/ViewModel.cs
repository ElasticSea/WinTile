using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using App.Model;
using App.Model.Entities;
using App.Model.Managers;
using App.Model.Managers.Strategies;
using App.Model.Managers.Window;
using App.Properties;
using Microsoft.Win32;
using PropertyChanged;
using Rect = App.Model.Entities.Rect;
using Window = App.Model.Entities.Window;

namespace App
{
    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly LayoutManager layoutManager = new LayoutManager();
        private bool _activeHotkeys;
        private Rect _selected;
        private HotkeyPair _selectedHotkeyPair;
        private SandboxWindowManager sandbox;
        private HotkeyManager hotkeyManager;
        private ConvertWindowManager nativeWindowManager;
        private readonly CompositeWindowManager windowManager = new CompositeWindowManager();
        private CuttingManager cuttingManager;
        private bool _enterSandboxMode;

        public ViewModel()
        {
            RegisterAppOnStartup(RunOnStartup);
        }

        public List<HotkeyType> HotkeyTypes
        {
            get
            {
                var used = Hotkeys.Select(h => h.Type);
                var types = Enum.GetValues(typeof(HotkeyType)).Cast<HotkeyType>();
                return types.Except(used).ToList();
            }
        }

        public string JsonLayout
        {
            get => layoutManager.Json;
            set
            {
                layoutManager.Json = value;
                Reload();
            }
        }

        private void Reload()
        {
            cuttingManager = new CuttingManager(layoutManager.Layout.Grid);
            nativeWindowManager = new ConvertWindowManager(new User32Manager());
            sandbox = new SandboxWindowManager(Tiles);
            windowManager.CurrentManager = nativeWindowManager;

            var move = new MoveStrategy(Tiles, windowManager);
            var select = new SelectStrategy(windowManager);
            var extend = new ExtendStrategy(Tiles, windowManager);
            //                var layout = new LayoutStrategy(Tiles, windowManager);

            hotkeyManager?.UnbindHotkeys();
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

            RegisterHotkeys(ActiveHotkeys);
            RegisterSandbox(EnterSandboxMode);
        }

        public Layout Layout => layoutManager.Layout;
        public ObservableCollection<Handle> Rows => layoutManager.Layout.Grid.Rows;
        public ObservableCollection<Handle> Columns => layoutManager.Layout.Grid.Columns;
        public ObservableCollection<Rect> Tiles => cuttingManager.Tiles;
        public ObservableCollection<HotkeyPair> Hotkeys => layoutManager.Layout.hotkeys;
        public ObservableCollection<Window> Windows => sandbox.Windows;

        public HotkeyPair SelectedHotkeyPair { private get; set; }
        public HotkeyType AddHotkeyType { get; set; }
        public Hotkey AddHotkeyHotkey { get; set; }

        public string AddHotkeyTypeTooltip
        {
            get
            {
                switch (AddHotkeyType)
                {
                    case HotkeyType.MoveLeft:
                        return "Moves current window left.";
                    case HotkeyType.MoveRight:
                        return "Moves current window right.";
                    case HotkeyType.MoveUp:
                        return "Moves current window up.";
                    case HotkeyType.MoveDown:
                        return "Moves current window down.";
                    case HotkeyType.ExpandLeft:
                        return "Expands current window left.";
                    case HotkeyType.ExpandRight:
                        return "Expands current window right.";
                    case HotkeyType.ExpandUp:
                        return "Expands current window up.";
                    case HotkeyType.ExpandDown:
                        return "Expands current window down.";
//                    case HotkeyType.LayoutLeft:
//                        return "";
//                    case HotkeyType.LayoutRight:
//                        return "";
//                    case HotkeyType.LayoutUp:
//                        return "";
//                    case HotkeyType.LayoutDown:
//                        return "";
                    case HotkeyType.SelectLeft:
                        return "Selects closest window that is left from current window.";
                    case HotkeyType.SelectRight:
                        return "Selects closest window that is right from current window.";
                    case HotkeyType.SelectUp:
                        return "Selects closest window that is up from current window.";
                    case HotkeyType.SelectDown:
                        return "Selects closest window that is down from current window.";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool ActiveHotkeys
        {
            get => _activeHotkeys;
            set
            {
                _activeHotkeys = value;

                RegisterHotkeys(value);
            }
        }

        private void RegisterHotkeys(bool active)
        {
            if (active)
                hotkeyManager.BindHotkeys();
            else
                hotkeyManager.UnbindHotkeys();
        }

        public bool EnterSandboxMode
        {
            get => _enterSandboxMode;
            set
            {
                _enterSandboxMode = value;

                RegisterSandbox(value);
            }
        }

        private void RegisterSandbox(bool active)
        {
            if (active)
            {
                ActiveHotkeys = true;
                windowManager.CurrentManager = sandbox;
            }
            else
            {
                windowManager.CurrentManager = nativeWindowManager;
            }
        }

        public void DefaultLayout() => JsonLayout = Resources.defaultProfile;

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public void AddWindow()
        {
            sandbox.AddWindow();
        }

        public void RemoveWindow()
        {
            sandbox.RemoveWindow();
        }

        public void AddHotkey()
        {
            if (AddHotkeyHotkey == null)
            {
                MessageBox.Show("Hotkey is empty. Click the Hotkey field and press desired hotkey combination.");
                return;
            }

            if (Hotkeys.Any(p => p.Hotkey == AddHotkeyHotkey))
            {
                MessageBox.Show("This hotkey is already taken.");
                return;
            }

            Hotkeys.Add(new HotkeyPair(AddHotkeyType, new Hotkey(AddHotkeyHotkey.Key, AddHotkeyHotkey.Modifiers)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HotkeyTypes)));
        }

        public void RemoveHotkey()
        {
            Hotkeys.Remove(SelectedHotkeyPair);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HotkeyTypes)));
        }

        public void CutVertical() => cuttingManager.CutVertical();
        public void CutHorizontal() => cuttingManager.CutHorizontal();

        public bool RunOnStartup
        {
            get => Settings.Default.RunOnStartup;
            set
            {
                Settings.Default.RunOnStartup = value;
                Settings.Default.Save();

                RegisterAppOnStartup(value);
            }
        }

        private void RegisterAppOnStartup(bool value)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                var curAssembly = Assembly.GetExecutingAssembly();
                var keyName = curAssembly.GetName().Name;

                if (value)
                {
                    key.SetValue(keyName, "\"" + curAssembly.Location + "\"- minimized");
                }
                else if (key.GetValue(keyName) != null)
                {
                    key.DeleteValue(keyName);
                }
            }
        }
    }
}
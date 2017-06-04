using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using App.Model;
using Microsoft.Win32;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Panel = System.Windows.Controls.Panel;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace App
{
    public partial class MainWindow : Window
    {
//        public static readonly DependencyProperty UsernameProperty =
//            DependencyProperty.Register(nameof(XTextBox), typeof(string), typeof(MainWindow),
//                new UIPropertyMetadata(string.Empty, Coolback));

        private readonly ViewModel viewModel = new ViewModel();
        private readonly Map<Tile, ToggleButton> Windows = new Map<Tile, ToggleButton>();

        public MainWindow()
        {
            KeyDown += (sender, args) =>
            {
//                var modifiers = new List<KeyModifier>();
//                var keyIsModifier = getModifier(args.Key) != KeyModifier.None;
//
//                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))  modifiers.Add(KeyModifier.Ctrl);
//                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) modifiers.Add(KeyModifier.Alt);
//                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))modifiers.Add(KeyModifier.Shift);
//                if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin)) modifiers.Add(KeyModifier.Win);
//
//                Console.WriteLine(string.Join(", ", modifiers) +" " + args.Key);
//                if (modifiers.Any() && keyIsModifier == false)
//                {
//                    if (PreTile.IsFocused) viewModel.PrevTile = new Hotkey(args.Key, modifiers);
//                    if (NextTile.IsFocused) viewModel.NextTile = new Hotkey(args.Key, modifiers);
//                    if (CurrentTile.IsFocused) viewModel.SelectedHotkey = new Hotkey(args.Key, modifiers);
//                }
            };

            SizeChanged += (sender, args) =>
            {
                foreach (var keyValuePair in Windows)
                    activateToggle(keyValuePair.Key, keyValuePair.Value);
            };

            InitializeComponent();

            var iconHandle = Properties.Resources.icon.GetHicon();

            var ni = new NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(iconHandle),
                Visible = true
            };

            ni.DoubleClick += (sender, args) =>
            {
                Show();
                WindowState = WindowState.Normal;
            };

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
                viewModel.Tiles.CollectionChanged += (sender, args) =>
                {
                    Canvas.Children.Clear();
                    buildToggles();
                };
                buildToggles();
            }

            InstallMeOnStartUp();
        }

        private KeyModifier getModifier(Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return KeyModifier.Ctrl;

                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.System:
                    return KeyModifier.Alt;

                case Key.LeftShift:
                case Key.RightShift:
                    return KeyModifier.Shift;

                case Key.LWin:
                case Key.RWin:
                    return KeyModifier.Win;
            }
            return KeyModifier.None;
        }

        private static void Coolback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.Print("OldValue: {0}", e.OldValue);
            Debug.Print("NewValue: {0}", e.NewValue);
        }

        private void InstallMeOnStartUp()
        {
            try
            {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                var curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            catch
            {
            }
        }

        private void buildToggles()
        {
            foreach (var tile in viewModel.Tiles)
            {
                var button = new ToggleButton {Background = Brushes.Transparent};
                activateToggle(tile, button);

                Windows.Add(tile, button);

                button.Click += (sender1, args1) => { selectWindow(button); };

                Canvas.Children.Add(button);
            }
        }

        private void AddWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.AddTile();
        }

        private void selectWindow(ToggleButton button)
        {
            if (CurrentToggle() == button && button.IsChecked == false)
            {
                viewModel.Selected = null;
            }
            else
            {
                // Deselect other buttons
                DeselectToggles(button);

                viewModel.Selected = Windows.Reverse[button];
            }
        }

        private void DeselectToggles(ToggleButton button)
        {
            Panel.SetZIndex(button, 1000);
            Windows.Values.ToList()
                .Where(b => b != button)
                .ForEach(b =>
                {
                    b.IsChecked = false;
                    Panel.SetZIndex(b, 0);
                });
        }

        private void activateToggle(Tile window, ToggleButton button)
        {
            var scaledWindow = window.Rect.extend((int) Canvas.ActualWidth, (int) Canvas.ActualHeight) / 100;
            Canvas.SetLeft(button, scaledWindow.Left);
            Canvas.SetTop(button, scaledWindow.Top);
            button.Width = scaledWindow.Width;
            button.Height = scaledWindow.Height;

//            if (window.Hotkey != null)
//            {
//                var content = $"[{window.Hotkey.key}] {string.Join(" ", window.Hotkey.modifiers)}";
//                button.Content = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content);
//            }
        }

        private void WindowChanged(Tile window)
        {
            var toggle = CurrentToggle();
            if (toggle != null) activateToggle(window, toggle);
        }

        // minimize to system tray when applicaiton is minimized
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                viewModel.hotkeyManager.Resume();
            }
            if (WindowState == WindowState.Normal)
            {
                viewModel.hotkeyManager.Pause();
            }

            base.OnStateChanged(e);
        }

        // minimize to system tray when applicaiton is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            // setting cancel to true will cancel the close request
            // so the application is not closed
            e.Cancel = true;

            Hide();
            viewModel.hotkeyManager.Resume();

            base.OnClosing(e);
        }

        private ToggleButton CurrentToggle()
        {
            return Windows.Values.FirstOrDefault(b => b.IsChecked == true);
        }

        private void RemoveWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var toggle = CurrentToggle();
            if (toggle != null) viewModel.RemoveTile(Windows.Reverse[toggle]);
        }

        private void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = "Wintile Layout Profile",
                DefaultExt = ".json",
                Filter = "Json Files(*.json)|*.json|All(*.*)|*"
            };

            if (dlg.ShowDialog() == true)
                File.WriteAllText(dlg.FileName, viewModel.JsonLayout);
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                FileName = "Wintile Layout Profile",
                DefaultExt = ".json",
                Filter = "Json Files(*.json)|*.json|All(*.*)|*"
            };

            if (dlg.ShowDialog() == true)
                viewModel.JsonLayout = File.ReadAllText(dlg.FileName);
        }

        private void SaveLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

        private void ResetLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.Reload();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var toggle = Windows.Forward[e.AddedItems[0] as Tile];
                toggle.IsChecked = true;
                DeselectToggles(toggle);
            }
        }

        private void ReorderTileUp_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.MoveSelectedUp();
        }

        private void ReorderTileDown_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.MoveSelectedDown();
        }

        private void CurrentTile_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            tryToAssignHotkey(args, hotkey => viewModel.SelectedHotkey = hotkey);
        }

        private void NextTile_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            tryToAssignHotkey(args, hotkey => viewModel.NextTile = hotkey);
        }

        private void PreTile_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
             tryToAssignHotkey(args, hotkey => viewModel.PrevTile = hotkey);
        }

        private void tryToAssignHotkey(KeyEventArgs args, Action<Hotkey> callback)
        {
            var modifiers = getActiveKeyModifiers();
            var keyIsModifier = getModifier(args.Key) != KeyModifier.None;

            if (args.Key == Key.Delete)
            {
                callback(null);
            }
            if (modifiers.Any() && keyIsModifier == false)
            {
                callback(new Hotkey(args.Key, modifiers.Aggregate((a, b) => a | b)));
            }
        }

        private List<KeyModifier> getActiveKeyModifiers()
        {
            var modifiers = new List<KeyModifier>();
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) modifiers.Add(KeyModifier.Ctrl);
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt) || Keyboard.IsKeyDown(Key.System)) modifiers.Add(KeyModifier.Alt);
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) modifiers.Add(KeyModifier.Shift);
            if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin)) modifiers.Add(KeyModifier.Win);
            return modifiers;
        }
    }
}
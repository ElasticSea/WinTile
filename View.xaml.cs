using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using WinTile.Model;

namespace WinTile
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel viewModel = new ViewModel();
        private readonly Map<WindowTile, ToggleButton> Windows = new Map<WindowTile, ToggleButton>();

        public MainWindow()
        {
            KeyDown += (sender, args) =>
            {
                var modifiers = new List<KeyModifier>();

                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    modifiers.Add(KeyModifier.Ctrl);
                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) modifiers.Add(KeyModifier.Alt);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    modifiers.Add(KeyModifier.Shift);
                if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin)) modifiers.Add(KeyModifier.Win);

                if (modifiers.Any())
                    viewModel.TriggerHotkeyChanged(args.Key, modifiers);
            };

            SizeChanged += (sender, args) =>
            {
                foreach (var keyValuePair in Windows)
                    activateToggle(keyValuePair.Key, keyValuePair.Value);
            };

            viewModel.WindowAdded += AddWindow;
            viewModel.WindowRemoved += removeWindow;
            viewModel.WindowChanged += WindowChanged;

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = viewModel;

            Loaded += (sender, args) => { viewModel.Load(); };
        }

        private void AddWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.AddWindow();
        }

        private void AddWindow(WindowTile window)
        {
            var button = new ToggleButton {Background = Brushes.Transparent};
            activateToggle(window, button);

            Windows.Add(window, button);

            button.Click += (sender, args) => { selectWindow(button); };

            Canvas.Children.Add(button);
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
                Panel.SetZIndex(button, 1000);
                Windows.Values.ToList()
                    .Where(b => b != button)
                    .ForEach(b =>
                    {
                        b.IsChecked = false;
                        Panel.SetZIndex(b, 0);
                    });

                viewModel.Selected = Windows.Reverse[button];
            }
        }

        private void removeWindow(WindowTile window)
        {
            var toggle = Windows.Forward[window];
            Canvas.Children.Remove(toggle);
        }

        private void activateToggle(WindowTile window, ToggleButton button)
        {
            var scaledWindow = window.rect.extend(Canvas.ActualWidth, Canvas.ActualHeight);
            Canvas.SetLeft(button, scaledWindow.Left);
            Canvas.SetTop(button, scaledWindow.Top);
            button.Width = scaledWindow.Width;
            button.Height = scaledWindow.Height;

            if (window.hotkey != null)
            {
                var content = $"[{window.hotkey.key}] {string.Join(" ", window.hotkey.modifiers)}";
                button.Content = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content);
            }
        }

        private void WindowChanged(WindowTile window)
        {
            var toggle = CurrentToggle();
            if (toggle != null) activateToggle(window, toggle);
        }

        private ToggleButton CurrentToggle()
        {
            return Windows.Values.FirstOrDefault(b => b.IsChecked == true);
        }

        private void RemoveWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var toggle = CurrentToggle();
            if (toggle != null) viewModel.removeWindow(Windows.Reverse[toggle]);
        }

        private void PrevWindow_OnClick(object sender, RoutedEventArgs e)
        {
            SelectNextToggle(-1);
        }

        private void NextWindow_OnClick(object sender, RoutedEventArgs e)
        {
            SelectNextToggle(1);
        }

        private void SelectNextToggle(int direction)
        {
            var toggles = Windows.Values
                .OrderBy(key =>
                {
                    var tile = Windows.Reverse[key].rect;
                    return tile.Left + tile.Top;
                })
                .ToList();

            var index = toggles.FindIndex(b => b.IsChecked == true);
            var toggle = toggles[(index + direction + toggles.Count) % toggles.Count];
            toggle.IsChecked = true;
            toggle.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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
            throw new NotImplementedException();
        }

        private void ApplyDimensions_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.TriggerTileChanged();
        }
    }
}
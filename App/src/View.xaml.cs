using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using App.Model;
using Microsoft.Win32;

namespace App
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(nameof(XTextBox), typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty, Coolback));

        private static void Coolback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.Print("OldValue: {0}", e.OldValue);
            Debug.Print("NewValue: {0}", e.NewValue);
        }

        private readonly ViewModel viewModel = new ViewModel();
        private readonly Map<Tile, ToggleButton> Windows = new Map<Tile, ToggleButton>();

        public MainWindow()
        {
//            KeyDown += (sender, args) =>
//            {
//                var modifiers = new List<KeyModifier>();
//
//                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
//                    modifiers.Add(KeyModifier.Ctrl);
//                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) modifiers.Add(KeyModifier.Alt);
//                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
//                    modifiers.Add(KeyModifier.Shift);
//                if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin)) modifiers.Add(KeyModifier.Win);
//
//                if (modifiers.Any())
//                        viewModel.TriggerHotkeyChanged(args.Key, modifiers);
//            };

            SizeChanged += (sender, args) =>
            {
                foreach (var keyValuePair in Windows)
                    activateToggle(keyValuePair.Key, keyValuePair.Value);
            };

            InitializeComponent();

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
            var scaledWindow = window.Rect.extend((int)Canvas.ActualWidth, (int)Canvas.ActualHeight) / 100;
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

        private ToggleButton CurrentToggle()
        {
            return Windows.Values.FirstOrDefault(b => b.IsChecked == true);
        }

        private void RemoveWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var toggle = CurrentToggle();
            if (toggle != null) viewModel.RemoveTile(Windows.Reverse[toggle]);
        }

        private void PrevWindow_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.PrevTile();
        }

        private void NextWindow_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.NextTile();
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
                File.WriteAllText(dlg.FileName, viewModel.export());
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
                viewModel.import(File.ReadAllText(dlg.FileName));
        }

        private void SaveLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

        private void ResetLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.reload();
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
    }
}
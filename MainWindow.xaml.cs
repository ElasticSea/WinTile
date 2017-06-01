using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinTile.Model;

namespace WinTile
{
    public partial class MainWindow : Window
    {
        private ViewModel viewModel = new ViewModel();
        private Map<WindowTile, ToggleButton> Windows = new Map<WindowTile, ToggleButton>();

        public MainWindow()
        {
            KeyDown += (sender, args) =>
            {
                viewModel.updateHotKey(args);
            };

            viewModel.WindowAdded += AddWindow;
            viewModel.WindowRemoved += removeWindow;
            viewModel.WindowChanged += WindowChanged;

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }

            Loaded += (sender, args) =>
            {
                viewModel.Load();
            };
        }

        private void AddWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.AddWindow();
        }

        private void AddWindow(WindowTile window)
        {

            var button = new ToggleButton();
            activateToggle(window, button);

            Windows.Add(window, button);

            button.Click += (sender, args) =>
            {
                selectWindow(button);
            };

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
                Canvas.SetZIndex(button, 1000);
                Windows.Values.ToList()
                    .Where(b => b != button)
                    .ForEach(b =>
                    {
                        b.IsChecked = false;
                        Canvas.SetZIndex(b, 0);
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
            var scaledWindow = window.tile.extend(Canvas.ActualWidth, Canvas.ActualHeight);
            Canvas.SetLeft(button, scaledWindow.Left);
            Canvas.SetTop(button, scaledWindow.Top);
            button.Width = scaledWindow.Width;
            button.Height = scaledWindow.Height;
            button.Content = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(window.hotkey.ToString());
        }

        private void WindowChanged(WindowTile window)
        {
            var toggle = CurrentToggle();
            if(toggle!=null) activateToggle(window, toggle);
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

        private void ApplyLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
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
                .OrderBy( key =>
                {
                    var tile = Windows.Reverse[key].tile;
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
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = "Wintile Layout Profile",
                DefaultExt = ".json", 
                Filter = "Json Files(*.json)|*.json|All(*.*)|*"
            };

            if (dlg.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(dlg.FileName, viewModel.JsonLayout);
            }
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
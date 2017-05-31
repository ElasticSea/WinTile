using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            rescaleToggle(window, button);

            Windows.Add(window, button);

            button.Click += (sender, args) =>
            {
                if (sender == button && button.IsChecked == false)
                {
                    viewModel.Selected = null;
                }
                else
                {
                    // Deselect other buttons
                    Windows.Values.ToList()
                        .Where(b => b != button)
                        .ForEach(b => b.IsChecked = false);

                    viewModel.Selected = Windows.Reverse[button];
                }
            };

            Canvas.Children.Add(button);
        }

        private void removeWindow(WindowTile window)
        {
            var toggle = Windows.Forward[window];
            Canvas.Children.Remove(toggle);
        }

        private void rescaleToggle(WindowTile window, ToggleButton button)
        {
            var scaledWindow = window.tile.extend(Canvas.ActualWidth, Canvas.ActualHeight);
            Canvas.SetLeft(button, scaledWindow.Left);
            Canvas.SetTop(button, scaledWindow.Top);
            button.Width = scaledWindow.Width;
            button.Height = scaledWindow.Height;
        }

        private void WindowChanged(WindowTile window)
        {
            var toggle = CurrentToggle();
            if(toggle!=null) rescaleToggle(window, toggle);
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
    }
}
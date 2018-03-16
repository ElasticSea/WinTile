using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using App.Model.Entities;
using App.Model.Managers;
using App.Utils;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;

namespace App.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ViewModel Vm
        {
            get => DataContext as ViewModel;
            set
            {
                if (!DesignerProperties.GetIsInDesignMode(this) && DataContext == null)
                {
                    value.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(ViewModel.JsonLayout))
                        {
                            DataContext = null;
                            DataContext = value;
                        }
                    };
                }
                DataContext = value;
            }
        }

        private void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = "Wintile Layout",
                DefaultExt = ".json",
                Filter = "Json Files(*.json)|*.json|All(*.*)|*"
            };

            if (dlg.ShowDialog() == true)
                File.WriteAllText(dlg.FileName, Vm.JsonLayout);
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                FileName = "Wintile Layout",
                DefaultExt = ".json",
                Filter = "Json Files(*.json)|*.json|All(*.*)|*"
            };

            if (dlg.ShowDialog() == true)
                Vm.JsonLayout = File.ReadAllText(dlg.FileName);
        }

        private void RemoveWindow(object sender, RoutedEventArgs e) => Vm.RemoveWindow();
        private void AddWindow(object sender, RoutedEventArgs e) => Vm.AddWindow();
        private void DefaultLayout(object sender, RoutedEventArgs e) => Vm.DefaultLayout();
        private void AddHotkey(object sender, RoutedEventArgs e) => Vm.AddHotkey();
        private void RemoveHotkey(object sender, RoutedEventArgs e) => Vm.RemoveHotkey();
        private void CutVertical(object sender, RoutedEventArgs e) => Vm.CutVertical();
        private void CutHorizontal(object sender, RoutedEventArgs e) => Vm.CutHorizontal();
        private void BindHotkeys(object sender, RoutedEventArgs e) => Vm.ActiveHotkeys = true;
        private void UnbindHotkeys(object sender, RoutedEventArgs e) => Vm.ActiveHotkeys = false;
        private void EnterEditMode(object sender, RoutedEventArgs e) => Vm.EnterSandboxMode = false;
        private void EnterSandboxMode(object sender, RoutedEventArgs e) => Vm.EnterSandboxMode = true;

        private void Hotkey_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            HotkeyBinding.assignHotkey(args, h => Vm.AddHotkeyHotkey = h);
        }

        private void onVerticalHandler(object sender, DragDeltaEventArgs e)
        {
            moveHandle(sender, e, e.VerticalChange / Canvaz.ActualHeight);
        }

        private void onHorizontalHandler(object sender, DragDeltaEventArgs e)
        {
            moveHandle(sender, e, e.HorizontalChange / Canvaz.ActualWidth);
        }

        private void moveHandle(object sender, DragDeltaEventArgs e, double value)
        {
            var handle = (sender as FrameworkElement).DataContext as Handle;
            handle.Position = (double) (handle.Position + value).Clamp(0, 1);
        }

        private void RemoveHandle(object sender, RoutedEventArgs e)
        {
            var handle = (sender as FrameworkElement).DataContext as Handle;
            Vm.Rows.Remove(handle);
            Vm.Columns.Remove(handle);
        }

        private void HandleOnFocus(object sender, RoutedEventArgs e)
        {
            (e.OriginalSource as TextBox)?.SelectAll();
        }

        private void HandlePreviewMouse(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!textBox.IsKeyboardFocusWithin)
            {
                // If the text box is not yet focused, give it the focus and
                // stop further processing of this click event.
                textBox.Focus();
                e.Handled = true;
            }
        }
    }
}
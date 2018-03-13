using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace App
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel VM = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
        
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                VM.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(VM.JsonLayout))
                    {
                        DataContext = null;
                        DataContext = VM;
                    }
                };
                VM.Load();
            }
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
                File.WriteAllText(dlg.FileName, VM.JsonLayout);
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
                VM.JsonLayout = File.ReadAllText(dlg.FileName);
        }

        private void RemoveWindow(object sender, RoutedEventArgs e) => VM.RemoveWindow();
        private void AddWindow(object sender, RoutedEventArgs e) => VM.AddWindow();
        private void SaveLayout(object sender, RoutedEventArgs e) => VM.Save();
        private void ResetLayout(object sender, RoutedEventArgs e) => VM.Load();
        private void AddHotkey(object sender, RoutedEventArgs e) => VM.AddHotkey();
        private void RemoveHotkey(object sender, RoutedEventArgs e) => VM.RemoveHotkey();
        private void CutVertical(object sender, RoutedEventArgs e) => VM.CutVertical();
        private void CutHorizontal(object sender, RoutedEventArgs e) => VM.CutHorizontal();

        private void Hotkey_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            HotkeyBinding.assignHotkey(args, h => VM.AddHotkeyHotkey = h);
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
            handle.Position = (float) (handle.Position + value).Clamp(0, 1);
        }

        private void RemoveHandle(object sender, RoutedEventArgs e)
        {
            var handle = (sender as FrameworkElement).DataContext as Handle;
            VM.Rows.Remove(handle);
            VM.Columns.Remove(handle);
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
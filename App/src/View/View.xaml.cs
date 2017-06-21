using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Microsoft.Win32;
using ContextMenu = System.Windows.Controls.ContextMenu;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace App
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel VM = new ViewModel();
        private readonly NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(Properties.Resources.icon.GetHicon()),
                Visible = true
            };
            notifyIcon.DoubleClick += (sender, args) =>
            {
                Show();
                WindowState = WindowState.Normal;
            };
            notifyIcon.MouseClick += (sender, args) =>
            {
                if (args.Button == MouseButtons.Right)
                    ((ContextMenu) FindResource("NotifierContextMenu")).IsOpen = true;
            };

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

            InstallMeOnStartUp();
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

        // minimize to system tray when applicaiton is minimized
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                VM.BindHotkeys();
            }
            if (WindowState == WindowState.Normal)
                VM.UnbindHotkeys();

            base.OnStateChanged(e);
        }

        // minimize to system tray when applicaiton is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            // setting cancel to true will cancel the close request
            // so the application is not closed
            e.Cancel = true;

            Hide();
            VM.BindHotkeys();

            base.OnClosing(e);
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

        private void RemoveWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.RemoveWindow();
        }

        private void AddWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.AddWindow();
        }

        private void SaveLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.Save();
        }

        private void ResetLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.Load();
        }

        private void Hotkey_OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            HotkeyBinding.assignHotkey(args, h => VM.AddHotkeyHotkey = h);
        }

        private void AddHotkeyButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.AddHotkey();
        }

        private void RemoveHotkeyButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.RemoveHotkey();
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void onVerticalHandler(object sender, DragDeltaEventArgs e)
        {
            moveHandle(sender, e, e.VerticalChange / Canvaz.ActualHeight);
        }

        private void onHorizontalHandler(object sender, DragDeltaEventArgs e)
        {
            moveHandle(sender, e, e.HorizontalChange / Canvaz.ActualWidth);
        }

        private void CutVerticalButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.CutVertical();
        }

        private void CutHorizontalButton_OnClick(object sender, RoutedEventArgs e)
        {
            VM.CutHorizontal();
        }

        private void moveHandle(object sender, DragDeltaEventArgs e, double value)
        {
            var handle = (sender as FrameworkElement).DataContext as Handle;
            handle.Value = (float)(handle.Value + value).Clamp(0, 1);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var handle = (sender as FrameworkElement).DataContext as Handle;
            VM.VerticalHandlers.Remove(handle);
            VM.HorizontalHandlers.Remove(handle);
        }
    }
}
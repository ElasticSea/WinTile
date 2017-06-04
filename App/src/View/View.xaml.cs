﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using Binding = System.Windows.Data.Binding;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Rect = App.Model.Rect;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace App
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel VM = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();

            var ni = new NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(Properties.Resources.icon.GetHicon()),
                Visible = true
            };
            ni.DoubleClick += (sender, args) =>
            {
                Show();
                WindowState = WindowState.Normal;
            };

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = VM;
                SizeChanged += (sender, args) =>
                {
                    Canvas.Children.Clear();
                    var button = new Rectangle() { Fill = Brushes.DimGray };
                    var canvasRect = new Rect(0, 0, (int)Canvas.ActualWidth, (int)Canvas.ActualHeight);
                    BindingOperations.SetBinding(button, Canvas.LeftProperty,
                        createRectBindable("Selected.Rect.Left", canvasRect.Width / 100f));
                    BindingOperations.SetBinding(button, Canvas.TopProperty,
                        createRectBindable("Selected.Rect.Top", canvasRect.Height / 100f));
                    BindingOperations.SetBinding(button, WidthProperty,
                        createRectBindable("Selected.Rect.Width", canvasRect.Width / 100f));
                    BindingOperations.SetBinding(button, HeightProperty,
                        createRectBindable("Selected.Rect.Height", canvasRect.Height / 100f));
                    Canvas.Children.Add(button);
                };
            }

            InstallMeOnStartUp();
        }

        private Binding createRectBindable(string name, float scale) => new Binding
        {
            Source = VM,
            Path = new PropertyPath(name),
            Mode = BindingMode.OneWay,
            Converter = new Multiplier(scale),
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

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
            {
                VM.UnbindHotkeys();
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


        private void RemoveWindowButton_OnClick(object sender, RoutedEventArgs e) => VM.RemoveTile(VM.Selected);
        private void AddWindowButton_OnClick(object sender, RoutedEventArgs e) => VM.AddTile();
        private void SaveLayoutButton_OnClick(object sender, RoutedEventArgs e) => VM.Save();
        private void ResetLayoutButton_OnClick(object sender, RoutedEventArgs e) => VM.Reload();
        private void ReorderTileUp_OnClick(object sender, RoutedEventArgs e) => VM.MoveSelectedUp();
        private void ReorderTileDown_OnClick(object sender, RoutedEventArgs e) => VM.MoveSelectedDown();
        private void CurrentTile_OnPreviewKeyDown(object sender, KeyEventArgs args) => HotKeyUtils.assignHotkey(args, h => VM.SelectedHotkey = h);
        private void NextTile_OnPreviewKeyDown(object sender, KeyEventArgs args) => HotKeyUtils.assignHotkey(args, h => VM.NextTile = h);
        private void PreTile_OnPreviewKeyDown(object sender, KeyEventArgs args) => HotKeyUtils.assignHotkey(args, h => VM.PrevTile = h);

      
    }
}
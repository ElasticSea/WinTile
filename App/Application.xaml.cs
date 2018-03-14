using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using App.Properties;
using Microsoft.Win32;

namespace App
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class Application
    {
        private NotifyIcon notifyIcon;
        private bool isExit;
        private ViewModel vm;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            vm = new ViewModel { JsonLayout = Settings.Default.Layout ?? App.Properties.Resources.defaultProfile };

            notifyIcon = new NotifyIcon();
            notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            notifyIcon.Icon = Icon.FromHandle(App.Properties.Resources.icon.GetHicon());
            notifyIcon.Visible = true;

            CreateContextMenu();
            RunOnStartup();
            ScanForChanges();
        }

        private void ScanForChanges()
        {
            bool active;
            string init;

            Activated += (sender, args) =>
            {
                init = vm.JsonLayout;
                active = true;
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    while (active)
                    {
                        if (vm != null && active)
                        {
                            var current = vm.JsonLayout;
                            if (current != init)
                            {
                                Settings.Default.Layout = current;
                                Settings.Default.Save();
                                init = current;
                            }
                        }

                        Thread.Sleep(300);
                    }
                }).Start();
            };

            Deactivated += (sender, args) =>
            {
                active = false;
            };
        }

        private void CreateContextMenu()
        {
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Settings").Click += (s, e) => ShowMainWindow();
            notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            isExit = true;
            MainWindow?.Close();
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            ((MainWindow)MainWindow).Vm = vm;
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Exception.ToString(), Assembly.GetEntryAssembly().GetName().Name);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

        private void RunOnStartup()
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
    }
}
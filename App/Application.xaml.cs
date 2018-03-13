using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            notifyIcon = new NotifyIcon();
            notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            notifyIcon.Icon = Icon.FromHandle(App.Properties.Resources.icon.GetHicon());
            notifyIcon.Visible = true;

            CreateContextMenu();
            RunOnStartup();
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
            MainWindow.Close();
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void ShowMainWindow()
        {
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
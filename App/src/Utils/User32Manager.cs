using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rect = App.Model.Rect;

namespace App.Utils
{
    public class User32Manager : IWindowManager
    {
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;

        public IntPtr getCurrentWindow() => GetForegroundWindow();

        public Rect getRectForWindow(IntPtr hwnd)
        {
            var windowRect = new NativeRect();
            GetWindowRect(hwnd, ref windowRect);

            return new Rect(windowRect.Left, windowRect.Top, windowRect.Right, windowRect.Bottom);
        }

        public void MoveWindow(IntPtr handle, Rect rect)
        {
            var windowRect = new NativeRect();
            GetWindowRect(handle, ref windowRect);
            var clientRect = new NativeRect();
            GetClientRect(handle, ref clientRect);

            var dx = Math.Min((windowRect.Right - windowRect.Left) - clientRect.Right, 16);
            var dy = Math.Min((windowRect.Bottom - windowRect.Top) - clientRect.Bottom, 8);

            SetWindowPos(handle, 0, rect.Left - dx / 2, rect.Top, rect.Width + dx, rect.Height + dy,
                SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        public IEnumerable<IntPtr> getVisibleWIndows()
        {
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (IsWindowVisible(wnd)) windows.Add(wnd);
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public void Focus(IntPtr hww) => SetForegroundWindow(hww);

        private struct NativeRect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref NativeRect rectangle);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, ref NativeRect rectangle);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
    }
}
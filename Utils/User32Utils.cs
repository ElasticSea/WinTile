using System;
using System.Runtime.InteropServices;

public static class User32Utils
{
    const short SWP_NOZORDER = 0X4;
    const int SWP_SHOWWINDOW = 0x0040;

    public static void SetCurrentWindowPos(int x, int y, int cx, int cy)
    {
        var handle = GetForegroundWindow();

        var windowRect = new Rect();
        GetWindowRect(handle, ref windowRect);
        var clientRect = new Rect();
        GetClientRect(handle, ref clientRect);

        var dx = Math.Min((windowRect.Right - windowRect.Left) - clientRect.Right, 16);
        var dy = Math.Min((windowRect.Bottom - windowRect.Top) - clientRect.Bottom, 8);

        if (handle != IntPtr.Zero)
        {
            SetWindowPos(handle, 0, x - dx/2, y, cx + dx, cy + dy, SWP_NOZORDER | SWP_SHOWWINDOW);
        }
    }

    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hwnd, ref Rect rectangle);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();
}
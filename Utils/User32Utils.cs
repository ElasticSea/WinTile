using System;
using System.Runtime.InteropServices;

public static class User32Utils
{
    const short SWP_NOSIZE = 1;
    const short SWP_NOZORDER = 0X4;
    const int SWP_SHOWWINDOW = 0x0040;

    public static void SetCurrentWindowPos(int x, int y, int cx, int cy)
    {
        var handle = GetForegroundWindow();
        if (handle != IntPtr.Zero)
        {
            SetWindowPos(handle, 0, x, y, cx, cy, SWP_NOZORDER | SWP_SHOWWINDOW);
        }
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();
}
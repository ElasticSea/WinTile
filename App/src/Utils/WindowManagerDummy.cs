using App.Model;

namespace App
{
    public class WindowManagerDummy : IWindowManager
    {
        public Rect CurrentWindowRect { get; set; }
    }
}
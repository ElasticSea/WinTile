using Rect = App.Model.Rect;

namespace App.Utils
{
    public class User32Manager : IWindowManager
    {
        public Rect CurrentWindowRect
        {
            get => User32Utils.GetCurrentWindoRect();
            set => User32Utils.MoveCurrentWindowToRect(value);
        }
    }
}
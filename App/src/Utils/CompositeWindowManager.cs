using App.Model;

namespace App.Utils
{
    public class CompositeWindowManager : IWindowManager
    {
        private readonly IWindowManager User32Manager;
        private readonly IWindowManager WindowManagerDummy;

        public bool Active;
                    
        public CompositeWindowManager(IWindowManager user32Manager, IWindowManager windowManagerDummy)
        {
            User32Manager = user32Manager;
            WindowManagerDummy = windowManagerDummy;
        }

        public Rect CurrentWindowRect
        {
            get => CurrentManager.CurrentWindowRect;
            set => CurrentManager.CurrentWindowRect = value;
        }

        private IWindowManager CurrentManager => Active ? User32Manager : WindowManagerDummy;
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using ElasticSea.Wintile.Model.Entities;

namespace ElasticSea.Wintile.Model.Managers
{
    public class HotkeyBinding : IDisposable
    {
        public const int WmHotKey = 0x0312;
        private static Dictionary<int, HotkeyBinding> _dictHotKeyToCalBackProc;

        private bool _disposed;

        public HotkeyBinding(Key k, KeyModifier keyModifiers, Action<HotkeyBinding> action, bool register = true)
        {
            Key = k;
            KeyModifiers = keyModifiers;
            Action = action;
            if (register)
                Bind();
        }

        public static Dictionary<int, HotkeyBinding> DictHotKeyToCalBackProc
        {
            get
            {
                if (_dictHotKeyToCalBackProc == null)
                {
                    _dictHotKeyToCalBackProc = new Dictionary<int, HotkeyBinding>();
                    ComponentDispatcher.ThreadFilterMessage += ComponentDispatcherThreadFilterMessage;
                }

                return _dictHotKeyToCalBackProc;
            }
        }

        public Key Key { get; }
        public KeyModifier KeyModifiers { get; }
        public Action<HotkeyBinding> Action { get; }
        public int virtualKeyCode => KeyInterop.VirtualKeyFromKey(Key);
        public int Id => virtualKeyCode + (int) KeyModifiers * 0x10000;

        // ******************************************************************
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public bool Bind()
        {
            var result = RegisterHotKey(IntPtr.Zero, Id, (uint) KeyModifiers, (uint) virtualKeyCode);

            DictHotKeyToCalBackProc.Add(Id, this);

            Debug.Print($"{result}, {Id}, {virtualKeyCode}");
            return result;
        }

        public void Unbind()
        {
            UnregisterHotKey(IntPtr.Zero, Id);
            DictHotKeyToCalBackProc.Remove(Id);
        }

        private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (handled) return;
            if (msg.message != WmHotKey) return;
            if (!DictHotKeyToCalBackProc.TryGetValue((int) msg.wParam, out var hotkeyBinding)) return;

            hotkeyBinding.Action?.Invoke(hotkeyBinding);
            handled = true;
        }

        // ******************************************************************
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be _disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be _disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                    Unbind();

                // Note disposing has been done.
                _disposed = true;
            }
        }

        public static void assignHotkey(KeyEventArgs args, Action<Hotkey> callback)
        {
            var modifiers = getActiveKeyModifiers();
            var keyIsModifier = getModifier(args.Key) != KeyModifier.None;

            if (args.Key == Key.Delete)
                callback(null);
            if (modifiers.Any() && keyIsModifier == false)
                callback(new Hotkey(args.Key, modifiers.Aggregate((a, b) => a | b)));
        }

        public static KeyModifier getModifier(Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return KeyModifier.Ctrl;

                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.System:
                    return KeyModifier.Alt;

                case Key.LeftShift:
                case Key.RightShift:
                    return KeyModifier.Shift;

                case Key.LWin:
                case Key.RWin:
                    return KeyModifier.Win;
            }

            return KeyModifier.None;
        }

        public static List<KeyModifier> getActiveKeyModifiers()
        {
            var modifiers = new List<KeyModifier>();
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) modifiers.Add(KeyModifier.Ctrl);
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt) || Keyboard.IsKeyDown(Key.System))
                modifiers.Add(KeyModifier.Alt);
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) modifiers.Add(KeyModifier.Shift);
            if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin)) modifiers.Add(KeyModifier.Win);
            return modifiers;
        }
    }

// ******************************************************************
    [Flags]
    public enum KeyModifier
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        NoRepeat = 0x4000,
        Shift = 0x0004,
        Win = 0x0008
    }
}
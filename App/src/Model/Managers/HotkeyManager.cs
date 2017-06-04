using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model.Managers
{
    public class HotkeyManager
    {
        private readonly List<HotKeyUtils> hotkeys = new List<HotKeyUtils>();

        public bool Register(Hotkey hotkey, Action<HotKeyUtils> action)
        {
            var conflict = hotkeys.FirstOrDefault(h => h.Key == hotkey.key && h.KeyModifiers == hotkey.modifiers);

            if (conflict == null)
            {
                hotkeys.Add(new HotKeyUtils(hotkey.key, hotkey.modifiers, action, false));
                return true;
            }
            return false;
        }

        public bool Unregister(Hotkey hotkey)
        {
            var conflict = hotkeys.FirstOrDefault(h => h.Key == hotkey.key && h.KeyModifiers == hotkey.modifiers);

            if (conflict != null)
            {
                hotkeys.Remove(conflict);
                return true;
            }
            return false;
        }

        public void Pause()
        {
            foreach (var hotkey in hotkeys)
            {
                hotkey.Unregister();
            }
        }

        public void Resume()
        {
            foreach (var hotkey in hotkeys)
            {
                hotkey.Register();
            }
        }
    }
}
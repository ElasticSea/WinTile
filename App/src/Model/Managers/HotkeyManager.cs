using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model.Managers
{
    public class HotkeyManager
    {
        private readonly IList<HotkeyPair> hotkeys;
        private readonly Dictionary<HotkeyType, Action<object>> mapping;

        private IEnumerable<HotkeyBinding> bindings;
        private bool binded;

        public HotkeyManager(IList<HotkeyPair> hotkeys, Dictionary<HotkeyType, Action<object>> mapping)
        {
            this.hotkeys = hotkeys;
            this.mapping = mapping;
        }

        public void UnbindHotkeys()
        {
            if (binded)
            {
                foreach (var hotkey in bindings)
                {
                    hotkey.Unbind();
                }
                binded = false;
            }
        }

        public void BindHotkeys()
        {
            if (binded == false)
            {
                bindings = from typeHotkey in hotkeys
                    where typeHotkey.Hotkey != null
                    select new HotkeyBinding(typeHotkey.Hotkey.Key, typeHotkey.Hotkey.Modifiers, mapping[typeHotkey.Type], false);

                foreach (var binding in bindings)
                {
                    binding.Bind();
                }
                binded = true;
            }
        }
    }
}
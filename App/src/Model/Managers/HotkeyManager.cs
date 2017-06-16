using System;
using System.Collections.Generic;
using System.Linq;
using App.Model.Managers.Strategies;

namespace App.Model.Managers
{
    public class HotkeyManager
    {
        private readonly Layout layout;
        private IEnumerable<HotkeyBinding> bindings;
        private bool binded;

        private readonly Dictionary<HotkeyType, Action<object>> mapping;

        public HotkeyManager(Layout layout, Dictionary<HotkeyType, Action<object>> mapping)
        {
            this.layout = layout;
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
                bindings = from typeHotkey in layout.hotkeys
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
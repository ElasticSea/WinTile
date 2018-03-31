using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSea.Wintile.Model.Entities;

namespace ElasticSea.Wintile.Model.Managers
{
    public class HotkeyManager
    {
        private readonly IList<HotkeyPair> hotkeys;
        private readonly Dictionary<HotkeyType, Action<object>> mapping;

        private IEnumerable<HotkeyBinding> bindings = new List<HotkeyBinding>();

        public HotkeyManager(IList<HotkeyPair> hotkeys, Dictionary<HotkeyType, Action<object>> mapping)
        {
            this.hotkeys = hotkeys;
            this.mapping = mapping;
        }

        public void UnbindHotkeys()
        {
            foreach (var hotkey in bindings)
                hotkey.Unbind();
        }

        public void BindHotkeys()
        {
            UnbindHotkeys();
            bindings = from typeHotkey in hotkeys
                       where typeHotkey.Hotkey != null
                       select new HotkeyBinding(typeHotkey.Hotkey.Key, typeHotkey.Hotkey.Modifiers,
                           mapping[typeHotkey.Type], false);

            foreach (var binding in bindings)
                binding.Bind();
        }
    }
}
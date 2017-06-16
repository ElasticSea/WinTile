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

        private readonly Dictionary<HotkeyType, Action<object>> Mapping;

        public HotkeyManager(Layout layout, ClosestStrategy closestStrategy, ExtendStrategy extendStrategy, LayoutStrategy layoutStrategy)
        {
            this.layout = layout;

            Mapping = new Dictionary<HotkeyType, Action<object>>
            {
                {HotkeyType.MoveLeft, h1 => closestStrategy.Left()},
                {HotkeyType.MoveRight, h1 => closestStrategy.Left()},
                {HotkeyType.MoveUp, h1 => closestStrategy.Left()},
                {HotkeyType.MoveDown, h1 => closestStrategy.Left()},

                {HotkeyType.ExpandLeft, h1 => extendStrategy.Left()},
                {HotkeyType.ExpandRight, h1 => extendStrategy.Left()},
                {HotkeyType.ExpandUp, h1 => extendStrategy.Left()},
                {HotkeyType.ExpandDown, h1 => extendStrategy.Left()},

                {HotkeyType.LayoutLeft, h1 => layoutStrategy.Left()},
                {HotkeyType.LayoutRight, h1 => layoutStrategy.Left()},
                {HotkeyType.LayoutUp, h1 => layoutStrategy.Left()},
                {HotkeyType.LayoutDown, h1 => layoutStrategy.Left()},
//
//                {HotkeyType.SelectLeft, h1 => closestStrategy.Left()},
//                {HotkeyType.SelectRight, h1 => closestStrategy.Left()},
//                {HotkeyType.SelectUp, h1 => closestStrategy.Left()},
//                {HotkeyType.SelectDown, h1 => closestStrategy.Left()}
            };
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
                    select new HotkeyBinding(typeHotkey.Hotkey.Key, typeHotkey.Hotkey.Modifiers, Mapping[typeHotkey.Type], false);

                foreach (var binding in bindings)
                {
                    binding.Bind();
                }
                binded = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using App.Model.Managers.Strategies;

namespace App.Model.Managers
{
    public class HotkeyManager
    {
        private readonly Layout layout;
        private IEnumerable<HotKeyUtils> hotkeys;
        private bool binded;
        private PrevNextStrategy prevNextStrategy;
        private ClosestStrategy closestStrategy;
        private ExtendStrategy extendStrategy;
        private ConcreteStrategy concreteStrategy;

        public HotkeyManager(Layout layout, PrevNextStrategy prevNextStrategy, ClosestStrategy closestStrategy, ExtendStrategy extendStrategy, ConcreteStrategy concreteStrategy)
        {
            this.layout = layout;
            this.prevNextStrategy = prevNextStrategy;
            this.closestStrategy = closestStrategy;
            this.extendStrategy = extendStrategy;
            this.concreteStrategy = concreteStrategy;
        }

        public void UnbindHotkeys()
        {
            if (binded)
            {
                foreach (var hotkey in hotkeys)
                {
                    hotkey.Unregister();
                }
                binded = false;
            }
        }

        public void BindHotkeys()
        {
            if (binded == false)
            {
                hotkeys = createHotkeys();

                foreach (var hotkey in hotkeys)
                {
                    hotkey.Register();
                }
                binded = true;
            }
        }

        private IEnumerable<HotKeyUtils> createHotkeys()
        {
            IEnumerable<HotKeyUtils> actual;
            var hotkeys = new List<HotKeyUtils>
            {
                create(layout.PreviousTile, h1 => prevNextStrategy.Prev()),
                create(layout.NextTile, h1 => prevNextStrategy.Next()),
                create(layout.ClosestRight, h1 => closestStrategy.Right()),
                create(layout.ClosestLeft, h1 => closestStrategy.Left()),
                create(layout.ClosestUp, h1 => closestStrategy.Up()),
                create(layout.ClosestDown, h1 => closestStrategy.Down()),
                create(layout.ExpandRight, h1 => extendStrategy.Right()),
                create(layout.ExpandLeft, h1 => extendStrategy.Left()),
                create(layout.ExpandUp, h1 => extendStrategy.Up()),
                create(layout.ExpandDown, h1 => extendStrategy.Down())
            };

            foreach (var tile in layout.tiles)
            {
                hotkeys.Add(create(tile.Hotkey, h1 => concreteStrategy.Position(tile)));
            }

            return hotkeys.Where(h => h != null);
        }

        private HotKeyUtils create(Hotkey hotkey, Action<object> action)
        {
            return hotkey != null ? new HotKeyUtils(hotkey.Key, hotkey.Modifiers, action, false) : null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model.Managers
{
    public class HotkeyManager
    {
        private readonly Layout layout;
        private IEnumerable<HotKeyUtils> hotkeys;
        private TileManager tm;
        private bool binded;

        public HotkeyManager(Layout layout, TileManager tm)
        {
            this.layout = layout;
            this.tm = tm;
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
                create(layout.PreviousTile, h1 => tm.PositionPrev()),
                create(layout.NextTile, h1 => tm.PositionNext()),
                create(layout.ClosestRight, h1 => tm.PositionClosestRight()),
                create(layout.ClosestLeft, h1 => tm.PositionClosestLeft()),
                create(layout.ClosestUp, h1 => tm.PositionClosestUp()),
                create(layout.ClosestDown, h1 => tm.PositionClosestDown())
            };

            foreach (var tile in layout.tiles)
            {
                hotkeys.Add(create(tile.Hotkey, h1 => tm.PositionWindow(tile)));
            }

            return hotkeys.Where(h => h != null);
        }

        private HotKeyUtils create(Hotkey hotkey, Action<object> action)
        {
            return hotkey != null ? new HotKeyUtils(hotkey.Key, hotkey.Modifiers, action, false) : null;
        }
    }
}
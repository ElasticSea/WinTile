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

        public HotkeyManager(Layout layout, TileManager tm)
        {
            this.layout = layout;
            this.tm = tm;
        }

        public void UnbindHotkeys()
        {
            foreach (var hotkey in hotkeys)
            {
                hotkey.Unregister();
            }
        }

        public void BindHotkeys()
        {
            hotkeys = createHotkeys();

            foreach (var hotkey in hotkeys)
            {
                hotkey.Register();
            }
        }

        private IEnumerable<HotKeyUtils> createHotkeys()
        {
            IEnumerable<HotKeyUtils> actual;
            var hotkeys = new List<HotKeyUtils>
            {
                create(layout.PreviousTile, h1 => tm.PositionPrev()),
                create(layout.NextTile, h1 => tm.PositionNext())
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
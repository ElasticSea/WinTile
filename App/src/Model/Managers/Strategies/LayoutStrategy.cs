using System;
using System.Collections.Generic;
using System.Linq;
using App.Utils;

namespace App.Model.Managers.Strategies
{
    public class LayoutStrategy : PositioningStrategy
    {
        public LayoutStrategy(SelectedHolder holder, IList<Tile> tiles, IWindowManager windowManager) : base(holder, tiles, windowManager)
        {
        }

        public void Left()
        {
            var value = Selected.Rect.Left;
            Move(value,-5, rect => rect.Left,(rect, i) => rect.Left = i);
            Move(value,-5, rect => rect.Right,(rect, i) => rect.Right = i);
        }

        public void Right()
        {
            var value = Selected.Rect.Right;
            Move(value, 5, rect => rect.Left, (rect, i) => rect.Left = i);
            Move(value, 5, rect => rect.Right, (rect, i) => rect.Right = i);
        }

        public void Up()
        {
            var value = Selected.Rect.Top;
            Move(value, -5, rect => rect.Top, (rect, i) => rect.Top = i);
            Move(value, -5, rect => rect.Bottom, (rect, i) => rect.Bottom = i);
        }

        public void Down()
        {
            var value = Selected.Rect.Bottom;
            Move(value, 5, rect => rect.Top, (rect, i) => rect.Top = i);
            Move(value, 5, rect => rect.Bottom, (rect, i) => rect.Bottom = i);
        }

        private void Move(int border, int amount, Func<Rect, int> get, Action<Rect, int> set)
        {
            var allwin = windowManager.getVisibleWIndows()
                .Select(t => new { Rect = windowManager.getRectForWindow(t), Handle = t });
            allwin.Where(a => get(a.Rect) == border).ForEach(a =>
            {
                set(a.Rect, get(a.Rect) + amount);
                windowManager.MoveWindow(a.Handle, a.Rect);
            });
            tiles.Where(t => get(t.Rect) == border).ForEach(t => set(t.Rect, get(t.Rect) + amount));
        }
    }
}
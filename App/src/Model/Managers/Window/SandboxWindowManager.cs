using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using App.Model;
using App.Model.Entities;

namespace App
{
    public class SandboxWindowManager : IWindowManager
    {
        private readonly Random rnd = new Random();
        private readonly ObservableCollection<Tile> tiles;

        public SandboxWindowManager(ObservableCollection<Tile> tiles)
        {
            this.tiles = tiles;
        }

        public ObservableCollection<Window> Windows { get; } = new ObservableCollection<Window>();

        public IntPtr FocusedWindow
        {
            get => new IntPtr(Windows.First(w => w.Selected).GetHashCode());
            set
            {
                Windows.ForEach(w => w.Selected = false);
                var tileFrom = GetTileFrom(value);
                tileFrom.Selected = true;

                var index = Windows.IndexOf(tileFrom);
                Windows.Move(index, Windows.Count - 1);
            }
        }

        public IEnumerable<IntPtr> GetVisibleWindows()
        {
            return Windows.Select(w => new IntPtr(w.GetHashCode()));
        }

        public Rect GetWindowRect(IntPtr handle)
        {
            return GetTileFrom(handle).Rect;
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            var tileFrom = GetTileFrom(handle);
            tileFrom.Rect = rect;
//            tileFrom.Rect.Right = rect.Right;
//            tileFrom.Rect.Top = rect.Top;
//            tileFrom.Rect.Bottom = rect.Bottom;
        }

        public Window GetTileFrom(IntPtr handle)
        {
            return Windows.First(w => w.GetHashCode() == handle.ToInt32());
        }

        public void AddWindow()
        {
            var tile = tiles.First();

            var brush = new SolidColorBrush(Color.FromArgb(255, (byte) rnd.Next(0, 128), (byte) rnd.Next(0, 128),
                (byte) rnd.Next(0, 128)));
            var window = new Window(false, brush, new Rect(tile.Rect));
            Windows.Add(window);

            if (Windows.Any(w => w.Selected) == false)
                Windows.First().Selected = true;
        }

        public void RemoveWindow()
        {
            var selected = Windows.FirstOrDefault(w => w.Selected);
            if (selected != null)
            {
                Windows.Remove(selected);
                Windows.FirstOrDefault()?.let(w => w.Selected = true);
            }
        }
    }
}
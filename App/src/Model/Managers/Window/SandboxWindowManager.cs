using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Model;
using App.Model.Entities;

namespace App
{
    public class SandboxWindowManager : IWindowManager
    {
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
            GetTileFrom(handle).Rect = rect;
        }

        public Window GetTileFrom(IntPtr handle)
        {
            return Windows.First(w => w.GetHashCode() == handle.ToInt32());
        }

        public void AddWindow()
        {
            Windows.Add(new Window(new Rect(tiles.First().Rect)));

            if (Windows.Any(w => w.Selected) == false)
                Windows.First().Selected = true;
        }

        public void RemoveWindow()
        {
            var selected = Windows.FirstOrDefault(w => w.Selected);
            if (selected != null)
            {
                Windows.Remove(selected);
                if (Windows.Any()) Windows.Last().Selected = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Model.Entities;
using App.Utils;

namespace App.Model.Managers.Window
{
    public class SandboxWindowManager : IWindowManager
    {
        private readonly ObservableCollection<Rect> tiles;

        public SandboxWindowManager(ObservableCollection<Rect> tiles)
        {
            this.tiles = tiles;
        }

        public ObservableCollection<Entities.Window> Windows { get; } = new ObservableCollection<Entities.Window>();

        public IntPtr? FocusedWindow
        {
            get
            {
                var window = Windows.FirstOrDefault(w => w.Selected);
                return window != null ? new IntPtr(window.GetHashCode()) : (IntPtr?)null;
            }
            set
            {
                Windows.ForEach(w => w.Selected = false);
                var tileFrom = GetTileFrom(value.Value);
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

        public Entities.Window GetTileFrom(IntPtr handle)
        {
            return Windows.First(w => w.GetHashCode() == handle.ToInt32());
        }

        public void AddWindow()
        {
            Windows.Add(new Entities.Window(new Rect(tiles.First())));

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
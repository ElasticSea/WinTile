using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using App.Model;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;

namespace App
{
    public class EditorWindowManager : IWindowManager
    {
        private IDictionary<Tile,Tile> binding = new Dictionary<Tile, Tile>();

        public EditorWindowManager(ObservableCollection<Tile> tiles)
        {
            tiles.CollectionChanged += (sender, args) =>
            {
                foreach(Tile tile in args.NewItems)
                {
                    var window = new Tile(new Rect(tile.Rect));
                    Windows.Add(window);
                    binding.Add(tile, window);
                }

                foreach (Tile tile in args.OldItems)
                {
                    var window = binding[tile];
                    binding.Remove(window);
                    Windows.Remove(window);
                }

                if (Windows.Any(w => w.Selected) == false)
                {
                    Windows.Last().Selected = true;
                }
            };

            tiles.ForEach(tile =>
                {
                    var window = new Tile(new Rect(tile.Rect)){Bursh = PickBrush()};
                    Windows.Add(window);
                    binding.Add(tile, window);
                }
            );

            if (Windows.Any(w => w.Selected) == false)
            {
                Windows.Last().Selected = true;
            }
        }
        Random rnd = new Random();
        private SolidColorBrush PickBrush()
        {

            var fromArgb = Color.FromArgb(255, (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256));
            return new SolidColorBrush(fromArgb);
        }

        public ObservableCollection<Tile> Windows { get; } = new ObservableCollection<Tile>();

        public IntPtr FocusedWindow
        {
            get => new IntPtr(Windows.First(w => w.Selected).GetHashCode());
            set
            {
                Windows.ForEach(w => w.Selected = false);
                var tileFrom = getTileFrom(value);
                tileFrom.Selected = true;

                var index = Windows.IndexOf(tileFrom);
                Windows.Move(index, Windows.Count - 1);
            }
        }

        public IEnumerable<IntPtr> GetVisibleWindows() => Windows.Select(w => new IntPtr(w.GetHashCode()));

        public Rect GetWindowRect(IntPtr handle)
        {
            return getTileFrom(handle).Rect;
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            var tileFrom = getTileFrom(handle);
            tileFrom.Rect = rect;
//            tileFrom.Rect.Right = rect.Right;
//            tileFrom.Rect.Top = rect.Top;
//            tileFrom.Rect.Bottom = rect.Bottom;
        }

        public Tile getTileFrom(IntPtr handle) => Windows.First(w => w.GetHashCode() == handle.ToInt32());
    }
}
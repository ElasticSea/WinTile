using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Model;

namespace App
{
    public class EditorWindowManager : IWindowManager
    {
        private Rect rect = new Rect();
        private IList<Tile> windows;

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
            };

            tiles.ForEach(tile =>
                {
                    var window = new Tile(new Rect(tile.Rect));
                    Windows.Add(window);
                    binding.Add(tile, window);
                }
            );
        }

        public IntPtr getCurrentWindow()
        {
            return IntPtr.Zero;
        }

        public ObservableCollection<Tile> Windows { get; } = new ObservableCollection<Tile>();

        public IntPtr FocusedWindow { get; set; }
        public IEnumerable<IntPtr> GetVisibleWindows() => new List<IntPtr>{ IntPtr.Zero };

        public Rect GetWindowRect(IntPtr handle)
        {
            return rect;
        }

        public void PositionWindow(IntPtr handle, Rect rect)
        {
            this.rect = rect;
        }

        public void Focus(IntPtr closesWqInd)
        {
            throw new NotImplementedException();
        }
    }
}
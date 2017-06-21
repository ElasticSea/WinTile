using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace App.Model.Managers
{
    public class CuttingManager
    {
        private ObservableCollection<Handle> verticalHandlers;
        private ObservableCollection<Handle> horizontalHandlers;

        public CuttingManager(ObservableCollection<Handle> verticalHandlers, ObservableCollection<Handle> horizontalHandlers)
        {
            this.verticalHandlers = verticalHandlers;
            this.horizontalHandlers = horizontalHandlers;

            verticalHandlers.CollectionChanged += (sender, args) =>
            {
                recalculate();
            };
            horizontalHandlers.CollectionChanged += (sender, args) =>
            {
                recalculate();
            };

            NotifyCollectionChangedEventHandler handlerChanged = (sender, args) => collectionPropertyChanged(args, (o, eventArgs) => recalculate());
            verticalHandlers.CollectionChanged += handlerChanged;
            horizontalHandlers.CollectionChanged += handlerChanged;

            recalculate();



        }

        public CuttingManager()
        {
            this.verticalHandlers = verticalHandlers;
            this.horizontalHandlers = horizontalHandlers;
        }

        public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();


        void collectionPropertyChanged(NotifyCollectionChangedEventArgs e, PropertyChangedEventHandler handler)
        {
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= handler;

            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += handler;
        }

        public void CutVertical() => cut(verticalHandlers);
        public void CutHorizontal() => cut(horizontalHandlers);

        private void cut(ObservableCollection<Handle> handles)
        {
            var dist = 0f;
            var start = 0f;
            var end = 1f;
            var values = handleValues(handles);
            for (var index = 0; index < values.Count - 1; index++)
            {
                var cur = values[index];
                var next = values[index + 1];
                var curDist = next - cur;

                if (curDist > dist)
                {
                    dist = curDist;
                    start = cur;
                    end = next;
                }
            }
            handles.Add(new Handle(start + (end - start) / 2));
        }

        public void recalculate()
        {
            Tiles.Clear();

            var hor = handleValues(horizontalHandlers);
            var ver = handleValues(verticalHandlers);

            for (var x = 0; x < hor.Count - 1; x++)
            {
                for (var y = 0; y < ver.Count - 1; y++)
                {
                    var left = hor[x];
                    var right = hor[x + 1];
                    var top = ver[y];
                    var bottom = ver[y + 1];

                    var rect = new Rect(left, top, right, bottom);
                    var tile = new Tile(rect);
                    Tiles.Add(tile);
                }
            }
        }

        private List<float> handleValues(IEnumerable<Handle> handles)
        {
            var vals = new List<float>();
            vals.Add(0);
            vals.AddRange(handles.Select(h => h.Value));
            vals.Add(1);
            vals.Sort();
            return vals;
        }
    }
}
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ElasticSea.Wintile.Model.Entities;
using ElasticSea.Wintile.Utils;

namespace ElasticSea.Wintile.Model.Managers
{
    public class CuttingManager
    {
        private Grid grid;

        public CuttingManager(Grid grid)
        {
            this.grid = grid;
            PropertyChangedEventHandler propertyChangedEventHandler = (o, eventArgs) => recalculate();
            NotifyCollectionChangedEventHandler handlerChanged = (sender, args) =>
                collectionPropertyChanged(sender, args, propertyChangedEventHandler);
            grid.Rows.CollectionChanged += handlerChanged;
            grid.Columns.CollectionChanged += handlerChanged;
            grid.Rows.Cast<INotifyPropertyChanged>().ForEach(r => r.PropertyChanged += propertyChangedEventHandler);
            grid.Columns.Cast<INotifyPropertyChanged>().ForEach(g => g.PropertyChanged += propertyChangedEventHandler);

            recalculate();
        }

        public ObservableCollection<Rect> Tiles { get; set; } = new ObservableCollection<Rect>();

        private void collectionPropertyChanged(object sender, NotifyCollectionChangedEventArgs e, PropertyChangedEventHandler handler)
        {
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= handler;

            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += handler;

            handler(sender, new PropertyChangedEventArgs(nameof(sender)));
        }

        public void CutVertical() => cut(grid.Columns);
        public void CutHorizontal() => cut(grid.Rows);

        private void cut(IList<Handle> handles)
        {
            var dist = 0.0;
            var start = 0.0;
            var end = 1.0;
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

        private void recalculate()
        {
            Tiles.Clear();

            var hor = handleValues(grid.Columns);
            var ver = handleValues(grid.Rows);

            for (var x = 0; x < hor.Count - 1; x++)
            {
                for (var y = 0; y < ver.Count - 1; y++)
                {
                    var left = hor[x];
                    var right = hor[x + 1];
                    var top = ver[y];
                    var bottom = ver[y + 1];

                    var rect = new Rect(left, top, right, bottom);
                    var tile = new Rect(rect);
                    Tiles.Add(tile);
                }
            }
        }

        private List<double> handleValues(IEnumerable<Handle> handles)
        {
            var vals = new List<double>();
            vals.Add(0);
            vals.AddRange(handles.Select(h => h.Position));
            vals.Add(1);
            vals.Sort();
            return vals;
        }
    }
}
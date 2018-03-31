using System.Collections.ObjectModel;

namespace ElasticSea.Wintile.Model.Entities
{
    public class Grid
    {
        public ObservableCollection<Handle> Rows { get; set; } = new ObservableCollection<Handle>();
        public ObservableCollection<Handle> Columns { get; set; } = new ObservableCollection<Handle>();
    }
}
using PropertyChanged;

namespace ElasticSea.Wintile.Model.Entities
{
    [ImplementPropertyChanged]
    public class Handle
    {
        public Handle(double position)
        {
            Position = position;
        }

        public double Position { get; set; }
    }
}
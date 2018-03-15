using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class Handle
    {
        public double Position { get; set; }

        public Handle(double position)
        {
            Position = position;
        }
    }
}
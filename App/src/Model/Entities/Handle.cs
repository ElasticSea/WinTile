using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class Handle
    {
        public float Position { get; set; }

        public Handle(float position)
        {
            Position = position;
        }
    }
}
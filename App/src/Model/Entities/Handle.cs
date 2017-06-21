using PropertyChanged;

namespace App
{
    [ImplementPropertyChanged]
    public class Handle
    {
        public float Value { get; set; }

        public Handle(float value)
        {
            Value = value;
        }
    }
}
using Newtonsoft.Json;
using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Rect
    {
        public Rect(Rect rect) : this(rect.Left, rect.Top, rect.Right, rect.Bottom)
        {
        }

        [JsonConstructor]
        public Rect(float left = 0, float top = 0, float right = 0, float bottom = 0)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        [JsonIgnore]
        public float Width
        {
            get => Right - Left;
            set => Right = value + Left;
        }

        [JsonIgnore]
        public float Height
        {
            get => Bottom - Top;
            set => Bottom = value + Top;
        }

        [JsonIgnore]
        public float Cx
        {
            get => Left + Width / 2;
            set
            {
                Left = value - Width / 2;
                Right = value + Width / 2;
            }
        }

        [JsonIgnore]
        public float Cy
        {
            get => Top + Height / 2;
            set
            {
                Top = value - Height / 2;
                Bottom = value + Height / 2;
            }
        }

        public Rect extend(float width, float height)
        {
            return new Rect(Left * width, Top * height, Right * width, Bottom * height);
        }

        public Rect shrink(float width, float height)
        {
            return new Rect(Left / width, Top / height, Right / width, Bottom / height);
        }

        protected bool Equals(Rect other)
        {
            return Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right) && Bottom.Equals(other.Bottom);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rect) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
                return hashCode;
            }
        }
    }
}
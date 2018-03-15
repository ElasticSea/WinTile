using System.Windows;
using Newtonsoft.Json;
using PropertyChanged;

namespace App.Model.Entities
{
    [ImplementPropertyChanged]
    public class Rect
    {
        public Rect(Rect rect) : this(rect.Left, rect.Top, rect.Right, rect.Bottom)
        {
        }

        [JsonConstructor]
        public Rect(double left = 0, double top = 0, double right = 0, double bottom = 0)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }

        [JsonIgnore]
        public Vector Size
        {
            get => new Vector(Right - Left, Bottom - Top);
            set
            {
                Right = value.X + Left;
                Bottom = value.Y + Top;
            }
        }

        [JsonIgnore]
        public Vector Center
        {
            get => new Vector(Left + Size.X / 2, Top + Size.Y / 2);
            set
            {
                Left = value.X - Size.X / 2;
                Right = value.X + Size.X / 2;
                Top = value.Y - Size.Y / 2;
                Bottom = value.Y + Size.Y / 2;
            }
        }

        public Rect Extend(Vector size)
        {
            return new Rect(Left * size.X, Top * size.Y, Right * size.X, Bottom * size.Y);
        }

        public Rect Shrink(Vector size)
        {
            return new Rect(Left / size.X, Top / size.Y, Right / size.X, Bottom / size.Y);
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

        public static bool operator ==(Rect left, Rect right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !Equals(left, right);
        }
    }
}
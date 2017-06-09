using Newtonsoft.Json;
using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Rect
    {
        private int _cx;
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        [JsonIgnore]
        public int Width
        {
            get => Right - Left;
            set => Right = value + Left;
        }

        [JsonIgnore]
        public int Height
        {
            get => Bottom - Top;
            set => Bottom = value + Top;
        }

        [JsonIgnore]
        public int Cx
        {
            get => Left +Width / 2;
            set
            {
                Left = value - Width / 2;
                Right = value + Width / 2;
            }
        }

        [JsonIgnore]
        public int Cy
        {
            get => Top +  Height / 2;
            set
            {
                Top = value - Height / 2;
                Bottom = value + Height / 2;
            }
        }

        public Rect(int left = 0, int top = 0, int right = 0, int bottom = 0)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rect extend(int width, int height)
        {
            return new Rect(Left * width, Top * height, Right * width, Bottom * height);
        }

        public static Rect operator *(Rect r0, int value)
        {
            return new Rect(r0.Left * value, r0.Top * value, r0.Right * value, r0.Bottom * value);
        }

        public static Rect operator /(Rect r0, int value)
        {
            return new Rect(r0.Left / value, r0.Top / value, r0.Right / value, r0.Bottom / value);
        }

        public override string ToString()
        {
            return $"{nameof(Left)}: {Left}, {nameof(Top)}: {Top}, {nameof(Right)}: {Right}, {nameof(Bottom)}: {Bottom}";
        }

        protected bool Equals(Rect other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
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
                var hashCode = Left;
                hashCode = (hashCode * 397) ^ Top;
                hashCode = (hashCode * 397) ^ Right;
                hashCode = (hashCode * 397) ^ Bottom;
                return hashCode;
            }
        }
    }
}
using Newtonsoft.Json;
using PropertyChanged;

namespace App.Model
{
    [ImplementPropertyChanged]
    public class Rect
    {
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
    }
}
namespace WinTile.Model
{
    public class Rect
    {
        public float Left, Top, Right, Bottom;

        public float Width
        {
            get => Right - Left;
            set => Right = value + Left;
        }

        public float Height
        {
            get => Bottom - Top;
            set => Bottom = value + Top;
        }

        public Rect(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rect extend(double width, double height)
        {
            return new Rect(Left * (float)width, Top * (float)height, Right * (float)width, Bottom * (float)height);
        }

        public static Rect operator *(Rect r0, float value)
        {
            return new Rect(r0.Left * value, r0.Top * value, r0.Right * value, r0.Bottom * value);
        }

        public static Rect operator /(Rect r0, float value)
        {
            return new Rect(r0.Left / value, r0.Top / value, r0.Right / value, r0.Bottom / value);
        }

        public override string ToString()
        {
            return $"{nameof(Left)}: {Left}, {nameof(Top)}: {Top}, {nameof(Right)}: {Right}, {nameof(Bottom)}: {Bottom}";
        }
    }
}
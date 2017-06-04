namespace App
{
    public class TilePositionManager
    {
        public virtual void MoveCurrentWindow(int x, int y, int width, int height)
        {
            User32Utils.SetCurrentWindowPos(x,y,width,height);
        }
    }
}
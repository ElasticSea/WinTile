namespace App
{
    public class TilePositionManager
    {
        public void MoveCurrentWindow(int x, int y, int width, int height)
        {
            User32Utils.SetCurrentWindowPos(x,y,width,height);
        }
    }
}
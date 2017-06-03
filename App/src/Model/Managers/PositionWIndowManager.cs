using System.Windows.Input;
using static System.Windows.SystemParameters;

namespace App.Model.Managers
{
    public class PositionWIndowManager
    {
        public PositionWIndowManager(TileManager tileManager)
        {
            new HotKeyUtils(
                Key.Right,
                KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt,
                k => {
                    tileManager.MoveNext();
                    if(tileManager.Selected!=null) PositionWindow(tileManager.Selected);
                });

            new HotKeyUtils(
                Key.Left,
                KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt,
                k => {
                    tileManager.MovePrev();
                    if (tileManager.Selected != null) PositionWindow(tileManager.Selected);
                });
        }

        private void PositionWindow(Tile tile)
        {
            var pxRect = tile.Rect.extend((int)WorkArea.Width, (int)WorkArea.Height) / 100;
            User32Utils.SetCurrentWindowPos(pxRect.Left, pxRect.Top, pxRect.Width, pxRect.Height);
        }
    }
}